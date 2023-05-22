using System.Net;
using System.Security.Cryptography;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameLevel CreateLevel(PublishLevelRequest request, GameUser user, bool createEvent = true, string? levelId = null)
    {
        levelId ??= GenerateLevelId();
        GameLevel level = new(levelId, user, AdhereToLevelNameCharacterLimit(request.Name), request.Language, request.FileSize, request.Created);

        _realm.Write(() =>
        {
            _realm.Add(level);
            user.LevelsCount = user.Levels.Count();
        });

        if (createEvent) CreateEvent(user, EventType.Publish, null, level);
        
        return level;
    }

    public GameLevel EditLevel(PublishLevelRequest updatedPublishLevel, GameLevel level)
    {
        _realm.Write(() =>
        {
            level.Name = AdhereToLevelNameCharacterLimit(updatedPublishLevel.Name);
            level.Language = updatedPublishLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
        });

        return level;
    }

    private void SetLevelPlayTime(GameLevel level)
    {
        // GameLevel doesn't have a LeaderboardEntry backlink since leaderboard entries only store the level id
        // (which is to support campaign levels)
        IQueryable<LeaderboardEntry> entriesOnLevel = _realm.All<LeaderboardEntry>().Where(e => e.LevelId == level.Id);
        long totalPlayTime = entriesOnLevel.AsEnumerable().Sum(e => e.PlayTime);

        _realm.Write(() =>
        {
            level.TotalPlayTime = totalPlayTime;
        });
    }

    public void UploadLevelResources(IDataStore dataStore, GameLevel level, byte[] levelFile, byte[] thumbnailFile,
        byte[] soundFile)
    {
        UploadLevelResource(dataStore, level, levelFile, FileType.Level);
        UploadLevelResource(dataStore, level, thumbnailFile, FileType.Image);
        UploadLevelResource(dataStore, level, soundFile, FileType.Sound);
    }

    private bool SetLevelInfo(GameLevel level, byte[] levelFile)
    {
        JObject? deCompressedLevel = LevelFileToJObject(levelFile);
        if (deCompressedLevel == null)
            return false;


        int bpm = deCompressedLevel.Value<int?>("bpm") ?? 120;
        int transposeValue = deCompressedLevel.Value<int?>("transposeValue") ?? 0;
        int scaleIndex = deCompressedLevel.Value<int?>("scaleIndex") ?? 0;
        int screensCount = (deCompressedLevel.GetValue("screenData") ?? throw new InvalidOperationException()).Count();

        int totalEntities = deCompressedLevel.GetValue("entities")?.Count() ?? 0;
        // This was added in later versions of the game, and it replaces entities
        int totalEntitiesB = deCompressedLevel.GetValue("entitiesB")?.Count() ?? 0;

        int entitiesCount = Math.Max(totalEntities, totalEntitiesB);

        _realm.Write(() =>
        {
            level.FileSize = levelFile.Length;
            level.Bpm = bpm;
            level.TransposeValue = transposeValue;
            level.ScaleIndex = scaleIndex;
            level.TotalScreens = screensCount;
            level.TotalEntities = entitiesCount;
        });

        return true;
    }
    
    public Response UploadLevelResource(IDataStore dataStore, GameLevel level,
        byte[] file, FileType fileType)
    {
        if (fileType == FileType.Image && !IsByteArrayPng(file))
            return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        string key = GetLevelResourceKey(level, fileType);
        dataStore.WriteToStore(key, file);
        
        SetLevelFilePath(level, fileType, key);

        if (fileType != FileType.Level) return HttpStatusCode.Created;
        return !SetLevelInfo(level, file) ? HttpStatusCode.BadRequest : HttpStatusCode.Created;
    }

    private void SetLevelFilePath(GameLevel level, FileType fileType, string path)
    {
        _realm.Write(() =>
        {
            switch (fileType)
            {
                case FileType.Level:
                    level.LevelFilePath = path;
                    break;
                case FileType.Image:
                    level.ThumbnailFilePath = path;
                    break;
                case FileType.Sound:
                    level.SoundFilePath = path;
                    break;
                case FileType.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        });
    }

    private static void RemoveLevelResources(GameLevel level, IDataStore dataStore)
    {
        if (level.LevelFilePath != null) dataStore.RemoveFromStore(level.LevelFilePath);
        if (level.ThumbnailFilePath != null) dataStore.RemoveFromStore(level.ThumbnailFilePath);
        if (level.SoundFilePath != null) dataStore.RemoveFromStore(level.SoundFilePath);
    }
    
    public void RemoveLevel(GameLevel level, IDataStore dataStore)
    {
        RemoveLevelResources(level, dataStore);

        RemoveAllReportsWithContentLevel(level);
        
        _realm.Write(() =>
        {
            foreach (GameAlbum album in level.Albums)
            {
                album.Levels.Remove(level);
            }
            _realm.RemoveRange(level.DailyLevels);
            _realm.RemoveRange(level.Likes);
            _realm.RemoveRange(_realm.All<LeaderboardEntry>().Where(e=>e.LevelId == level.Id));
            _realm.Remove(level);
        });
    }
    
    public GameLevel? GetLevelWithId(string id) => _realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);

    private IQueryable<GameLevel> GetLevelsWithIds(IEnumerable<string> ids)
    {
        List<GameLevel> levels = new ();
        
        foreach (string levelId in ids) 
        {
            GameLevel? level = GetLevelWithId(levelId);
            if (level != null) levels.Add(level);
        }

        return levels.AsQueryable();
    }

    public (GameLevel[], int) GetLevels(LevelOrderType order, bool descending, LevelFilters filters, int from, int count)
    {
        IQueryable<GameLevel> orderedLevels = order switch
        {
            LevelOrderType.CreationDate => LevelsOrderedByCreationDate(descending),
            LevelOrderType.ModificationDate => LevelsOrderedByModificationDate(descending),
            LevelOrderType.Plays => LevelsOrderedByPlays(descending),
            LevelOrderType.UniquePlays => LevelsOrderedByUniquePlays(descending),
            LevelOrderType.Likes => LevelsOrderedByLikes(descending),
            LevelOrderType.FileSize => LevelsOrderedByFileSize(descending),
            LevelOrderType.Difficulty => LevelsOrderedByDifficulty(descending),
            LevelOrderType.Relevance => LevelsOrderedByRelevance(descending),
            LevelOrderType.Random => LevelsOrderedByRandom(descending),
            LevelOrderType.TotalDeaths => LevelsOrderedByDeaths(descending),
            LevelOrderType.TotalPlayTime => LevelsOrderedByTotalPlayTime(descending),
            LevelOrderType.AveragePlayTime => LevelsOrderedByAveragePlayTime(descending),
            LevelOrderType.TotalScreens => LevelsOrderedByScreens(descending),
            LevelOrderType.TotalEntities => LevelsOrderedByEntities(descending),
            LevelOrderType.Bpm => LevelsOrderedByBpm(descending),
            LevelOrderType.TransposeValue => LevelsOrderedByTransposeValue(descending),
            _ => LevelsOrderedByCreationDate(descending)
        };

        IQueryable<GameLevel> filteredLevels = FilterLevels(orderedLevels, filters);
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(filteredLevels, from, count);

        return (paginatedLevels, filteredLevels.Count());
    }
    
    private IQueryable<GameLevel> FilterLevels(IQueryable<GameLevel> levels, LevelFilters filters)
    {
        IQueryable<GameLevel> response = levels;
        
        if (filters.ByUser != null)
        {
            response = response.Where(l => l.Author == filters.ByUser);
        }
        
        if (filters.LikedByUser != null)
        {
            IQueryable<LevelLikeRelation> relations = filters.LikedByUser.LikedLevels;

            List<GameLevel> tempResponse = new ();

            foreach (LevelLikeRelation relation in relations)
            {
                GameLevel likedLevel = relation.Level;
                GameLevel? responseLevel = response.FirstOrDefault(l => l.Id == likedLevel.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.InAlbum != null)
        {
            List<GameLevel> tempResponse = new ();

            foreach (GameLevel level in filters.InAlbum.Levels)
            {
                GameLevel? responseLevel = response.FirstOrDefault(l => l.Id == level.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.InDaily == true)
        {
            IQueryable<DailyLevel> dailyLevelObjects =
                GetDailyLevelObjects(DailyLevelOrderType.Date, true, new DailyLevelFilters());

            IEnumerable<GameLevel> filteredLevels = new List<GameLevel>();
            
            if (!dailyLevelObjects.Any() && filters.InDaily == false)
            {
                filteredLevels = levels;
            }
            
            foreach (DailyLevel dailyLevel in dailyLevelObjects)
            {
                string levelId = dailyLevel.Level.Id;
                filteredLevels = filteredLevels.Concat(levels.Where(l=> l .Id == levelId));
            }

            response = filteredLevels.AsQueryable();
        }
        
        if (filters.InDaily == false)
        {
            IQueryable<DailyLevel> dailyLevelObjects =
                GetDailyLevelObjects(DailyLevelOrderType.Date, true, new DailyLevelFilters());

            IEnumerable<GameLevel> filteredLevels = new List<GameLevel>();
            
            if (!dailyLevelObjects.Any())
            {
                filteredLevels = levels;
            }
            
            foreach (DailyLevel dailyLevel in dailyLevelObjects)
            {
                string levelId = dailyLevel.Level.Id;
                filteredLevels = filteredLevels.Concat(levels.Where(l=> l .Id != levelId));
            }

            response = filteredLevels.AsQueryable();
        }
        
        if (filters.InDailyDate != null || filters.InLatestDaily == true)
        {
            IQueryable<DailyLevel> dailyLevelObjects = GetDailyLevelObjects(DailyLevelOrderType.Date, true, new DailyLevelFilters(filters.InDailyDate, filters.InLatestDaily));

            List<GameLevel> tempResponse = new ();

            foreach (DailyLevel dailyLevelObject in dailyLevelObjects)
            {
                GameLevel dailyLevel = dailyLevelObject.Level;
                GameLevel? responseLevel = response.FirstOrDefault(l => l.Id == dailyLevel.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            response = tempResponse.AsQueryable();
        }

        if (filters.Search != null)
        {
            GameUser? userWithSearchName = GetUserWithUsername(filters.Search);
            response = response.Where(l => l.Name.Contains(filters.Search, StringComparison.OrdinalIgnoreCase) || l.Author == userWithSearchName);
        }

        if (filters.CompletedBy != null)
        {            
            List<GameLevel> tempResponse = new ();
            
            foreach (GameLevel level in filters.CompletedBy.CompletedLevels)
            {
                GameLevel? responseLevel = response.FirstOrDefault(l => l.Id == level.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            response = tempResponse.AsQueryable();
        }

        if (filters.Bpm != null)
        {
            response = response.Where(l => l.Bpm == filters.Bpm);
        }

        if (filters.Scale != null)
        {
            response = response.Where(l => l.ScaleIndex == (int)filters.Scale);
        }

        if (filters.TransposeValue != null)
        {
            response = response.Where(l => l.TransposeValue == filters.TransposeValue);
        }
        
        return response;
    }

    private IQueryable<GameLevel> LevelsOrderedByCreationDate(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.CreationDate);
        return _realm.All<GameLevel>().OrderBy(l => l.CreationDate);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByModificationDate(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.ModificationDate);
        return _realm.All<GameLevel>().OrderBy(l => l.ModificationDate);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByPlays(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.PlaysCount);
        return _realm.All<GameLevel>().OrderBy(l => l.PlaysCount);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByUniquePlays(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.UniquePlaysCount);
        return _realm.All<GameLevel>().OrderBy(l => l.UniquePlaysCount);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByLikes(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.LikesCount);
        return _realm.All<GameLevel>().OrderBy(l => l.LikesCount);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByFileSize(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.FileSize);
        return _realm.All<GameLevel>().OrderBy(l => l.FileSize);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByDifficulty(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.Difficulty);
        return _realm.All<GameLevel>().OrderBy(l => l.Difficulty);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByRelevance(bool descending)
    {
        DateTimeOffset oneWeekAgo = DateTimeOffset.UtcNow.AddDays(-7);
        IQueryable<LevelUniquePlayRelation> relations = _realm.All<LevelUniquePlayRelation>().Where(r=>r.Date > oneWeekAgo);

        var groupedRelations = relations
            .AsEnumerable()
            .GroupBy(r => r.Level)
            .Select(g => new {
                Level = g.Key,
                Count = g.Count()
            });

        if (descending) return groupedRelations.OrderByDescending(r => r.Count).Select(r => r.Level).AsQueryable();
        return groupedRelations.OrderBy(r => r.Count).Select(r => r.Level).AsQueryable();
    } 
    
    // TODO: Cache this every 24 hours
    private IQueryable<GameLevel> LevelsOrderedByRandom(bool descending)
    {
        DateTime seedDateTime = DateTime.Today;
        byte[] seedBytes = BitConverter.GetBytes(seedDateTime.Ticks);
        byte[] hashBytes = MD5.HashData(seedBytes);
        int seed = BitConverter.ToInt32(hashBytes, 0);

        Random rng = new(seed);
        
        if (descending) return _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(_ => rng.Next())
            .AsQueryable();
        
        return _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderBy(_ => rng.Next())
            .AsQueryable();
    }
    
    private IQueryable<GameLevel> LevelsOrderedByDeaths(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.TotalDeaths);
        return _realm.All<GameLevel>().OrderBy(l => l.TotalDeaths);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByTotalPlayTime(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.TotalPlayTime);
        return _realm.All<GameLevel>().OrderBy(l => l.TotalPlayTime);
    } 
    
    private IQueryable<GameLevel> LevelsOrderedByAveragePlayTime(bool descending)
    {
        // I AM SORRY TO WHOEVER IS READING THIS
        if (descending) return _realm.All<GameLevel>().AsEnumerable().OrderByDescending(l => l.TotalPlayTime != 0 && l.PlaysCount != 0 ? l.TotalPlayTime / l.PlaysCount : 0).AsQueryable();
        return _realm.All<GameLevel>().AsEnumerable().OrderBy(l => l.TotalPlayTime != 0 && l.PlaysCount != 0 ? l.TotalPlayTime / l.PlaysCount : 0).AsQueryable();
    }

    private IQueryable<GameLevel> LevelsOrderedByScreens(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.TotalScreens);
        return _realm.All<GameLevel>().OrderBy(l => l.TotalScreens);
    }
    
    private IQueryable<GameLevel> LevelsOrderedByEntities(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.TotalEntities);
        return _realm.All<GameLevel>().OrderBy(l => l.TotalEntities);
    }
    
    private IQueryable<GameLevel> LevelsOrderedByBpm(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.Bpm);
        return _realm.All<GameLevel>().OrderBy(l => l.Bpm);
    }
    
    private IQueryable<GameLevel> LevelsOrderedByTransposeValue(bool descending)
    {
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.TransposeValue);
        return _realm.All<GameLevel>().OrderBy(l => l.TransposeValue);
    }

    public void AddCompletionToLevel(GameUser user, GameLevel level)
    {
        if (!level.UniqueCompletions.Contains(user)) AddUniqueCompletion(user, level);
        
        _realm.Write(() =>
        {
            level.CompletionCount++;
        });
    }

    private void AddUniqueCompletion(GameUser user, GameLevel level)
    {
        _realm.Write(() =>
        {
            level.UniqueCompletions.Add(user);
            level.UniqueCompletionsCount = level.UniqueCompletions.Count;
            user.CompletedLevelsCount = user.CompletedLevels.Count();
        });
    }
    
    public void SetLevelDifficulty(GameLevel level)
    {
        _realm.Refresh();

        _realm.Write(() =>
        {
            level.Difficulty = CalculateLevelDifficulty(level);
        });
    }
    public void AddDeathsToLevel(GameUser user, GameLevel level, int deaths)
    {
        _realm.Write(() =>
        {
            level.TotalDeaths += deaths;
            user.Deaths += deaths;
        });
    }
}
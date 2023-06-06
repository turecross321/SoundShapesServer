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
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameLevel CreateLevel(GameUser user, PublishLevelRequest request, bool createEvent = true, string? levelId = null)
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

        bool hasCar = deCompressedLevel.GetValue("entityTypesUsed")
            ?.Values<string>().FirstOrDefault
                // ReSharper disable once StringLiteralTypo
                (e => e == "Platformer_EntityPacks_GameStuff_CarCheckpoint") != null;
        
        bool hasExplodingCar = deCompressedLevel.GetValue("entityTypesUsed")
            ?.Values<string>().FirstOrDefault
                // ReSharper disable once StringLiteralTypo
                (e => e == "Platformer_EntityPacks_GameStuff_ExplodingCarCheckpoint") != null;

        _realm.Write(() =>
        {
            level.FileSize = levelFile.Length;
            level.Bpm = bpm;
            level.TransposeValue = transposeValue;
            level.ScaleIndex = scaleIndex;
            level.TotalScreens = screensCount;
            level.TotalEntities = entitiesCount;
            level.HasCar = hasCar;
            level.HasExplodingCar = hasExplodingCar;
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
    
    public GameLevel? GetLevelWithId(string id) => _realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);

    private IQueryable<GameLevel> GetLevelsWithIds(IEnumerable<string> ids)
    {
        List<GameLevel> levels = new();
        
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (string levelId in ids) 
        {
            GameLevel? level = GetLevelWithId(levelId);
            if (level != null) levels.Add(level);
        }

        return levels.AsQueryable();
    }

    public (GameLevel[], int) GetLevels(LevelOrderType order, bool descending, LevelFilters filters, int from, int count)
    {
        IQueryable<GameLevel> levels = _realm.All<GameLevel>();
        IQueryable<GameLevel> filteredLevels = FilterLevels(levels, filters);
        IQueryable<GameLevel> orderedLevels = OrderLevels(filteredLevels, order, descending);
        
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(orderedLevels, from, count);

        return (paginatedLevels, filteredLevels.Count());
    }
    
    private IQueryable<GameLevel> FilterLevels(IQueryable<GameLevel> levels, LevelFilters filters)
    {
        IQueryable<GameLevel> response = levels;
        
        if (filters.ByUser != null)
        {
            response = response.Where(l => l.Author == filters.ByUser);
        }

        if (filters.LikedByUser != null || filters.QueuedByUser != null || filters.LikedOrQueuedByUser != null)
        {
            IEnumerable<LevelLikeRelation>? likeRelations = filters.LikedByUser?.LikedLevels;
            IEnumerable<LevelQueueRelation>? queueRelations = filters.QueuedByUser?.QueuedLevels;

            likeRelations ??= filters.LikedOrQueuedByUser?.LikedLevels;
            queueRelations ??= filters.LikedOrQueuedByUser?.QueuedLevels;

            // if null, make them empty
            likeRelations ??= Enumerable.Empty<LevelLikeRelation>();
            queueRelations ??= Enumerable.Empty<LevelQueueRelation>();
            
            IEnumerable<GameLevel> combinedLevels = likeRelations
                .Select(lR => new { lR.Level, lR.Date })
                .Concat(queueRelations.Select(qR => new { qR.Level, qR.Date }))
                .OrderByDescending(obj => obj.Date)
                .Select(obj => obj.Level);

            response = combinedLevels.AsQueryable();
        }

        if (filters.InAlbum != null)
        {
            List<GameLevel> tempResponse = new();

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
                GetDailyLevels(DailyLevelOrderType.Date, true, new DailyLevelFilters());

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
                GetDailyLevels(DailyLevelOrderType.Date, true, new DailyLevelFilters());

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
            IQueryable<DailyLevel> dailyLevelObjects = GetDailyLevels(DailyLevelOrderType.Date, true, new DailyLevelFilters(filters.InDailyDate, filters.InLatestDaily));

            List<GameLevel> tempResponse = new();

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
            List<GameLevel> tempResponse = new();
            
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

        if (filters.ScaleIndex != null)
        {
            response = response.Where(l => l.ScaleIndex == filters.ScaleIndex);
        }

        if (filters.TransposeValue != null)
        {
            response = response.Where(l => l.TransposeValue == filters.TransposeValue);
        }

        if (filters.HasCar != null)
        {
            response = response.Where(l => l.HasCar == filters.HasCar);
        }

        if (filters.HasExplodingCar != null)
        {
            response = response.Where(l => l.HasExplodingCar == filters.HasExplodingCar);
        }
        
        return response;
    }

    #region Level Ordering

    private IQueryable<GameLevel> OrderLevels(IQueryable<GameLevel> levels, LevelOrderType order, bool descending)
    {
        return order switch
        {
            LevelOrderType.CreationDate => OrderLevelsByCreationDate(levels, descending),
            LevelOrderType.ModificationDate => OrderLevelsByModificationDate(levels, descending),
            LevelOrderType.TotalPlays => OrderLevelsByPlays(levels, descending),
            LevelOrderType.UniquePlays => OrderLevelsByUniquePlays(levels, descending),
            LevelOrderType.TotalCompletions => OrderLevelsByCompletions(levels, descending),
            LevelOrderType.UniqueCompletions => OrderLevelsByUniqueCompletions(levels, descending),
            LevelOrderType.Likes => OrderLevelsByLikes(levels, descending),
            LevelOrderType.Queues => OrderLevelsByQueues(levels, descending),
            LevelOrderType.FileSize => OrderLevelsByFileSize(levels, descending),
            LevelOrderType.Difficulty => OrderLevelsByDifficulty(levels, descending),
            LevelOrderType.Relevance => OrderLevelsByRelevance(levels, descending),
            LevelOrderType.Random => OrderLevelsByRandom(levels, descending),
            LevelOrderType.TotalDeaths => OrderLevelsByDeaths(levels, descending),
            LevelOrderType.TotalPlayTime => OrderLevelsByPlayTime(levels, descending),
            LevelOrderType.AveragePlayTime => OrderLevelsByAveragePlayTime(levels, descending),
            LevelOrderType.TotalScreens => OrderLevelsByScreens(levels, descending),
            LevelOrderType.TotalEntities => OrderLevelsByEntities(levels, descending),
            LevelOrderType.Bpm => OrderLevelsByBpm(levels, descending),
            LevelOrderType.TransposeValue => OrderLevelsByTransposeValue(levels, descending),
            _ => levels
        };
    }
    
    private static IQueryable<GameLevel> OrderLevelsByCreationDate(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.CreationDate);
        return levels.OrderBy(l => l.CreationDate);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByModificationDate(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.ModificationDate);
        return levels.OrderBy(l => l.ModificationDate);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByPlays(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.PlaysCount);
        return levels.OrderBy(l => l.PlaysCount);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByUniquePlays(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.UniquePlaysCount);
        return levels.OrderBy(l => l.UniquePlaysCount);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByLikes(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.LikesCount);
        return levels.OrderBy(l => l.LikesCount);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByQueues(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.QueuesCount);
        return levels.OrderBy(l => l.QueuesCount);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByFileSize(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.FileSize);
        return levels.OrderBy(l => l.FileSize);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByDifficulty(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.Difficulty);
        return levels.OrderBy(l => l.Difficulty);
    } 
    
    private IQueryable<GameLevel> OrderLevelsByRelevance(IQueryable<GameLevel> levels, bool descending)
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

        groupedRelations = groupedRelations.Where(r => levels.AsEnumerable().Contains(r.Level));
        
        if (descending) return groupedRelations.OrderByDescending(r => r.Count).Select(r => r.Level).AsQueryable();
        return groupedRelations.OrderBy(r => r.Count).Select(r => r.Level).AsQueryable();
    } 
    
    // TODO: Cache this every 24 hours
    private static IQueryable<GameLevel> OrderLevelsByRandom(IQueryable<GameLevel> levels, bool descending)
    {
        DateTime seedDateTime = DateTime.Today;
        byte[] seedBytes = BitConverter.GetBytes(seedDateTime.Ticks);
        byte[] hashBytes = MD5.HashData(seedBytes);
        int seed = BitConverter.ToInt32(hashBytes, 0);

        Random rng = new(seed);
        
        if (descending) return levels
            .AsEnumerable()
            .OrderByDescending(_ => rng.Next())
            .AsQueryable();
        
        return levels
            .AsEnumerable()
            .OrderBy(_ => rng.Next())
            .AsQueryable();
    }
    
    private static IQueryable<GameLevel> OrderLevelsByDeaths(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.TotalDeaths);
        return levels.OrderBy(l => l.TotalDeaths);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByPlayTime(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.TotalPlayTime);
        return levels.OrderBy(l => l.TotalPlayTime);
    } 
    
    private static IQueryable<GameLevel> OrderLevelsByAveragePlayTime(IQueryable<GameLevel> levels, bool descending)
    {
        // I AM SORRY TO WHOEVER IS READING THIS
        if (descending) return levels.AsEnumerable().OrderByDescending(l => l.TotalPlayTime != 0 && l.PlaysCount != 0 ? l.TotalPlayTime / l.PlaysCount : 0).AsQueryable();
        return levels.AsEnumerable().OrderBy(l => l.TotalPlayTime != 0 && l.PlaysCount != 0 ? l.TotalPlayTime / l.PlaysCount : 0).AsQueryable();
    }

    private static IQueryable<GameLevel> OrderLevelsByScreens(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.TotalScreens);
        return levels.OrderBy(l => l.TotalScreens);
    }
    
    private static IQueryable<GameLevel> OrderLevelsByEntities(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.TotalEntities);
        return levels.OrderBy(l => l.TotalEntities);
    }
    
    private static IQueryable<GameLevel> OrderLevelsByBpm(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.Bpm);
        return levels.OrderBy(l => l.Bpm);
    }
    
    private static IQueryable<GameLevel> OrderLevelsByTransposeValue(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.TransposeValue);
        return levels.OrderBy(l => l.TransposeValue);
    }

    private static IQueryable<GameLevel> OrderLevelsByCompletions(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.CompletionCount);
        return levels.OrderBy(l => l.CompletionCount);
    }
    
    private static IQueryable<GameLevel> OrderLevelsByUniqueCompletions(IQueryable<GameLevel> levels, bool descending)
    {
        if (descending) return levels.OrderByDescending(l => l.UniqueCompletionsCount);
        return levels.OrderBy(l => l.UniqueCompletionsCount);
    }    

    #endregion
}
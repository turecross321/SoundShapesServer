using System.Security.Cryptography;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameLevel CreateLevel(PublishLevelRequest request, GameUser user, bool createEvent = true)
    {
        string levelId = GenerateLevelId();
        GameLevel level = new(levelId, user, request.Name, request.Language, request.FileSize, request.Created);

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
            level.Name = updatedPublishLevel.Name;
            level.Language = updatedPublishLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
        });

        return level;
    }

    public void SetLevelFileSize(GameLevel level, long fileSize)
    {
        _realm.Write(() =>
        {
            level.FileSize = fileSize;
        });
    }
    
    // Not database related, but idk where this should be otherwise.
    private static void RemoveLevelResources(GameLevel level, IDataStore dataStore)
    {
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Image));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Level));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Sound));
    }
    
    public void RemoveLevel(GameLevel level, IDataStore dataStore)
    {
        RemoveLevelResources(level, dataStore);

        RemoveAllReportsWithContentId(level.Id);
        
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
            LevelOrderType.Deaths => LevelsOrderedByDeaths(descending),
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
        if (filters.InDailyDate != null)
        {
            IQueryable<DailyLevel> dailyLevelObjects = GetDailyLevelObjects(filters.InDailyDate);

            List<GameLevel> tempResponse = new ();

            foreach (DailyLevel dailyLevel in dailyLevelObjects)
            {
                GameLevel? responseLevel = response.FirstOrDefault(l => l == dailyLevel.Level);
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
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.Plays);
        return _realm.All<GameLevel>().OrderBy(l => l.Plays);
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
        if (descending)
            return _realm.All<GameLevel>()
                .AsEnumerable()
                .OrderByDescending(l =>
                    l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5)
                .AsQueryable();
        
        return _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderBy(l =>
                l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5)
            .AsQueryable();
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
        if (descending) return _realm.All<GameLevel>().OrderByDescending(l => l.Deaths);
        return _realm.All<GameLevel>().OrderBy(l => l.Deaths);
    } 

    public void AddUniqueCompletion(GameLevel level, GameUser user)
    {
        if (level.UniqueCompletions.Contains(user)) return;

        _realm.Write(() =>
        {
            level.UniqueCompletions.Add(user);
            level.UniqueCompletionsCount = level.UniqueCompletions.Count;
            user.CompletedLevelsCount = user.CompletedLevels.Count();
        });
    }
    public void AddCompletionToLevel(GameLevel level)
    {
        _realm.Write(() =>
        {
            level.CompletionCount++;
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
    public void AddPlayToLevel(GameLevel level)
    {
        _realm.Write(() =>
        {
            level.Plays++;
        });
    }
    public void AddUniquePlayToLevel(GameLevel level, GameUser user)
    {
        if (level.UniquePlays.Contains(user)) return;
        
        _realm.Write(() =>
        {
            level.UniquePlays.Add(user);
            level.UniquePlaysCount = level.UniquePlays.Count;
            user.PlayedLevelsCount = user.PlayedLevels.Count();
        });
    }

    public void AddDeathsToLevel(GameUser user, GameLevel level, int deaths)
    {
        _realm.Write(() =>
        {
            level.Deaths += deaths;
            user.Deaths += deaths;
        });
    }
}
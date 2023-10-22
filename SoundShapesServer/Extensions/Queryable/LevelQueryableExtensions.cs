using System.Security.Cryptography;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class LevelQueryableExtensions
{
    public static IQueryable<GameLevel> FilterLevels(this IQueryable<GameLevel> levels, GameDatabaseContext database, LevelFilters filters, GameUser? accessor)
    {
        if (filters.CreatedAfter != null)
        {
            levels = levels.Where(l => l.CreationDate >= filters.CreatedAfter);
        }
        
        if (filters.InDailyDate != null || filters.InLatestDaily == true)
        {
            IEnumerable<DailyLevel> dailyLevelObjects = database.GetDailyLevels(DailyLevelOrderType.Date, true, new DailyLevelFilters{Date = filters.InDailyDate, LatestDate = filters.InLatestDaily});
            IEnumerable<GameLevel> temp = new List<GameLevel>();
            temp = dailyLevelObjects.Aggregate(temp, (current, d) => current.Append(d.Level));

            filters.InDailyDate = null;
            filters.InLatestDaily = null;
            return temp.AsQueryable().FilterLevels(database, filters, accessor);
        }
        
        if (filters.InDaily != null)
        {
            IEnumerable<DailyLevel> dailyLevelObjects = database.GetDailyLevels(DailyLevelOrderType.Date, true, new DailyLevelFilters());
            IEnumerable<GameLevel> temp = new List<GameLevel>();
            temp = dailyLevelObjects.Aggregate(temp, (current, d) => current.Append(d.Level));

            filters.InDaily = null;
            return temp.AsQueryable().FilterLevels(database, filters, accessor);
        }
        
        if (filters.ByUser != null)
        {
            levels = levels.Where(l => l.Author == filters.ByUser);
        }

        if (filters.LikedByUser != null || filters.QueuedByUser != null || filters.LikedOrQueuedByUser != null)
        {
            IEnumerable<LevelLikeRelation>? likeRelations = filters.LikedByUser?.LikedLevelRelations;
            IEnumerable<LevelQueueRelation>? queueRelations = filters.QueuedByUser?.QueuedLevelRelations;

            likeRelations ??= filters.LikedOrQueuedByUser?.LikedLevelRelations;
            queueRelations ??= filters.LikedOrQueuedByUser?.QueuedLevelRelations;

            // if null, make them empty
            likeRelations ??= Enumerable.Empty<LevelLikeRelation>();
            queueRelations ??= Enumerable.Empty<LevelQueueRelation>();
            
            IEnumerable<GameLevel> combinedLevels = likeRelations
                .Select(lR => new { lR.Level, lR.Date })
                .Concat(queueRelations.Select(qR => new { qR.Level, qR.Date }))
                .OrderByDescending(obj => obj.Date)
                .Select(obj => obj.Level);

            levels = combinedLevels.AsQueryable();
        }

        if (filters.InAlbum != null)
        {
            List<GameLevel> tempResponse = new();

            foreach (GameLevel level in filters.InAlbum.Levels)
            {
                GameLevel? responseLevel = levels.FirstOrDefault(l => l.Id == level.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            levels = tempResponse.AsQueryable();
        }

        if (filters.Search != null)
        {
            GameUser? user = database.GetUserWithUsername(filters.Search);
            levels = levels.Where(l => l.Name.Contains(filters.Search, StringComparison.OrdinalIgnoreCase) || l.Author == user);
        }

        if (filters.CompletedBy != null)
        {            
            List<GameLevel> tempResponse = new();
            
            foreach (GameLevel level in filters.CompletedBy.CompletedLevels)
            {
                GameLevel? responseLevel = levels.FirstOrDefault(l => l.Id == level.Id);
                if (responseLevel != null) tempResponse.Add(responseLevel);
            }

            levels = tempResponse.AsQueryable();
        }

        if (filters.Bpm != null)
        {
            levels = levels.Where(l => l.Bpm == filters.Bpm);
        }

        if (filters.ScaleIndex != null)
        {
            levels = levels.Where(l => l.ScaleIndex == filters.ScaleIndex);
        }

        if (filters.TransposeValue != null)
        {
            levels = levels.Where(l => l.TransposeValue == filters.TransposeValue);
        }

        if (filters.HasCar != null)
        {
            levels = levels.Where(l => l.HasCar == filters.HasCar);
        }

        if (filters.HasExplodingCar != null)
        {
            levels = levels.Where(l => l.HasExplodingCar == filters.HasExplodingCar);
        }

        if (filters.UploadPlatforms != null)
        {
            IEnumerable<GameLevel> tempLevels = new List<GameLevel>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (PlatformType platformType in filters.UploadPlatforms)
            {
                tempLevels = tempLevels.Concat(levels.Where(e=> e._UploadPlatform == (int)platformType));
            }

            levels = tempLevels.AsQueryable();
        }
        
        // Automatically remove unlisted and private levels from results
        if ((accessor?.PermissionsType ?? PermissionsType.Default) < PermissionsType.Moderator)
        {
            IQueryable<GameLevel> nonPublicLevels = levels.Where(l => l.Author != accessor && l._Visibility != (int)LevelVisibility.Public);
            levels = levels.AsEnumerable().Except(nonPublicLevels).AsQueryable();
        }
        
        return levels;
    }
    
    public static IQueryable<GameLevel> OrderLevels(this IQueryable<GameLevel> levels, LevelOrderType order, bool descending)
    {
        return order switch
        {
            LevelOrderType.CreationDate => levels.OrderByDynamic(l => l.CreationDate, descending),
            LevelOrderType.ModificationDate => levels.OrderByDynamic(l => l.ModificationDate, descending),
            LevelOrderType.TotalPlays => levels.OrderByDynamic(l => l.PlaysCount, descending),
            LevelOrderType.UniquePlays => levels.OrderByDynamic(l => l.UniquePlaysCount, descending),
            LevelOrderType.TotalCompletions => levels.OrderByDynamic(l => l.CompletionCount, descending),
            LevelOrderType.UniqueCompletions => levels.OrderByDynamic(l => l.UniqueCompletionsCount, descending),
            LevelOrderType.Likes => levels.OrderByDynamic(l => l.LikesCount, descending),
            LevelOrderType.Queues => levels.OrderByDynamic(l => l.QueuesCount, descending),
            LevelOrderType.FileSize => levels.OrderByDynamic(l => l.FileSize, descending),
            LevelOrderType.Difficulty => levels.OrderByDynamic(l => l.Difficulty, descending),
            LevelOrderType.Random => levels.OrderLevelsByRandom(descending),
            LevelOrderType.TotalDeaths => levels.OrderByDynamic(l => l.TotalDeaths, descending),
            LevelOrderType.TotalPlayTime => levels.OrderByDynamic(l => l.TotalPlayTime, descending),
            // add 1 to avoid cases where they are 0
            LevelOrderType.AveragePlayTime => levels.OrderByDynamic(l => (l.TotalPlayTime + 1) / (l.PlaysCount + 1), descending),
            LevelOrderType.TotalScreens => levels.OrderByDynamic(l => l.TotalScreens, descending),
            LevelOrderType.TotalEntities => levels.OrderByDynamic(l => l.TotalEntities, descending),
            LevelOrderType.Bpm => levels.OrderByDynamic(l => l.Bpm, descending),
            LevelOrderType.TransposeValue => levels.OrderByDynamic(l => l.TransposeValue, descending),
            _ => levels
        };
    }
    
    private static IQueryable<GameLevel> OrderLevelsByRandom(this IQueryable<GameLevel> levels, bool descending)
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
}
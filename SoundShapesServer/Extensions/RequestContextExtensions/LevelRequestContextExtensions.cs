using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Extensions.RequestContextExtensions;

public static class LevelRequestContextExtensions
{
        public static LevelFilters GetLevelFilters(this RequestContext context, GameDatabaseContext database)
    {
        return new LevelFilters
        {
            ByUser = context.QueryString["createdBy"].ToUser(database),
            LikedByUser = context.QueryString["likedBy"].ToUser(database),
            QueuedByUser = context.QueryString["queuedBy"].ToUser(database),
            LikedOrQueuedByUser = context.QueryString["likedOrQueuedBy"].ToUser(database),
            InAlbum = context.QueryString["inAlbum"].ToAlbum(database),
            InDaily = context.QueryString["inDaily"].ToBool(),
            InDailyDate = context.QueryString["inDailyDate"].ToDateFromUnix(),
            InLatestDaily = context.QueryString["inLatestDaily"].ToBool(),
            Search = context.QueryString["search"],
            CompletedBy = context.QueryString["completedBy"].ToUser(database),
            Bpm = context.QueryString["bpm"].ToInt(),
            ScaleIndex = context.QueryString["scaleIndex"].ToInt(),
            TransposeValue = context.QueryString["transposeValue"].ToInt(),
            HasCar = context.QueryString["hasCar"].ToBool(),
            HasExplodingCar = context.QueryString["hasExplodingCar"].ToBool(),
            HasUfo = context.QueryString["hasUfo"].ToBool(),
            HasFirefly = context.QueryString["hasFirefly"].ToBool(),
            UploadPlatforms = context.QueryString["uploadPlatforms"].ToEnumList<PlatformType>(),
            CreatedBefore = context.QueryString["createdBefore"].ToDateFromUnix(),
            CreatedAfter = context.QueryString["createdAfter"].ToDateFromUnix(),
            AnyCompletions = context.QueryString["anyCompletions"].ToBool() 
        };
    }
    
    public static LevelOrderType GetLevelOrderType(this RequestContext context)
    {
        return context.QueryString["orderBy"] switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "totalPlays" => LevelOrderType.TotalPlays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "totalCompletions" => LevelOrderType.TotalCompletions,
            "uniqueCompletions" => LevelOrderType.UniqueCompletions,
            "likes" => LevelOrderType.Likes,
            "queues" => LevelOrderType.Queues,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "random" => LevelOrderType.Random,
            "totalDeaths" => LevelOrderType.TotalDeaths,
            "totalPlayTime" => LevelOrderType.TotalPlayTime,
            "averagePlayTime" => LevelOrderType.AveragePlayTime,
            "totalScreens" => LevelOrderType.TotalScreens,
            "totalEntities" => LevelOrderType.TotalEntities,
            "bpm" => LevelOrderType.Bpm,
            "transposeValue" => LevelOrderType.TransposeValue,
            _ => LevelOrderType.DoNotOrder
        };
    }
}
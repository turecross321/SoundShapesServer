using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Extensions.RequestContextExtensions;

public static class LevelRequestContextExtensions
{
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
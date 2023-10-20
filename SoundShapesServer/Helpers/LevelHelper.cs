using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    private const string LevelIdCharacters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int LevelIdLength = 8;
    
    public static string GenerateLevelId()
    {
        Random r = new();
        string levelId = "";
        for (int i = 0; i < LevelIdLength; i++)
        {
            levelId += LevelIdCharacters[r.Next(LevelIdCharacters.Length - 1)];
        }

        return levelId;
    }

    public static float CalculateLevelDifficulty(GameLevel level)
    {
        // I know this is ugly, but this is authentic to the original servers, while also supporting decimals
        // which is used for sorting levels by difficulty.

        if (level.TotalDeaths == 0 || level.CompletionCount == 0) return 0;
        
        float averageAmountOfDeaths = (float)level.TotalDeaths / level.CompletionCount;
        
        switch (averageAmountOfDeaths)
        {
            case >= 30:
                return averageAmountOfDeaths / 6;
            case >= 20:
                return averageAmountOfDeaths / 5;
            case >= 10:
                return averageAmountOfDeaths / 3.3f;
        }

        if (averageAmountOfDeaths >= 2.5)
        {
            return averageAmountOfDeaths / 2.5f;
        }

        return 0;
    }

    private const int LevelNameCharacterLimit = 26;
    public static string AdhereToLevelNameCharacterLimit(string name)
    {
        return name[..Math.Min(name.Length, LevelNameCharacterLimit)];
    }

    public static bool IsUserAllowedToAccessLevel(GameLevel level, GameUser? user)
    {
        return !(level.Visibility == LevelVisibility.Private && level.Author.Id != user?.Id && user?.PermissionsType < PermissionsType.Moderator);
    }

    public static LevelOrderType GetLevelOrderType(RequestContext context)
    {
        string? orderString = context.QueryString["orderBy"];
        
        return orderString switch
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
    
    public static LevelFilters GetLevelFilters(RequestContext context, GameDatabaseContext database)
    {
        GameUser? byUser = context.QueryString["createdBy"].ToUser(database);
        GameUser? likedBy = context.QueryString["likedBy"].ToUser(database);
        GameUser? queuedBy =  context.QueryString["queuedBy"].ToUser(database);
        GameUser? likedOrQueuedBy = context.QueryString["likedOrQueuedBy"].ToUser(database);
        GameUser? completedBy = context.QueryString["completedBy"].ToUser(database);
        GameAlbum? inAlbum = context.QueryString["inAlbum"].ToAlbum(database);
        bool? inDaily = context.QueryString["inDaily"].ToBool();
        DateTimeOffset? inDailyDate = context.QueryString["inDailyDate"].ToDateFromUnix();
        bool? latestDaily = context.QueryString["inLatestDaily"].ToBool();
        int? bpm = context.QueryString["bpm"].ToInt();
        int? transposeValue = context.QueryString["transposeValue"].ToInt();
        int? scaleIndex = context.QueryString["scaleIndex"].ToInt();
        bool? hasCar = context.QueryString["hasCar"].ToBool();
        bool? hasExplodingCar = context.QueryString["hasExplodingCar"].ToBool();
        string? searchQuery = context.QueryString["search"];
        List<PlatformType>? uploadPlatforms = context.QueryString["uploadPlatforms"].ToEnumList<PlatformType>();
        DateTimeOffset? createdAfter = context.QueryString["createdAfter"].ToDateFromUnix();
        
        return new LevelFilters
        {
            ByUser = byUser,
            LikedByUser = likedBy,
            QueuedByUser = queuedBy,
            LikedOrQueuedByUser = likedOrQueuedBy,
            InAlbum = inAlbum,
            InDaily = inDaily,
            InDailyDate = inDailyDate,
            InLatestDaily = latestDaily,
            Search = searchQuery,
            CompletedBy = completedBy,
            Bpm = bpm,
            ScaleIndex = scaleIndex,
            TransposeValue = transposeValue,
            HasCar = hasCar,
            HasExplodingCar = hasExplodingCar,
            UploadPlatforms = uploadPlatforms,
            CreatedAfter = createdAfter
        };
    }

    public static readonly List<string> OfflineLevelIds = new()
    {
        "vic1_ver2",
        "vic2",
        "vic3",
        "vic4_master",
        "craig1",
        "craig2",
        "craig3",
        "craig4",
        "colinIce",
        "colinDesert",
        "colinFactory",
        "colinUnderwater",
        "colinUFO",
        "pixeljam1",
        "pixeljam2",
        "pixeljam3",
        "pixeljam4",
        "beckCities",
        "beckThePeople",
        "beckSpiralStaircase",
        "carTutorial",
        "carDLC",
        "carDLC_metal"
    };
}
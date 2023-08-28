using Bunkum.HttpServer;
using SoundShapesServer.Database;
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
            "relevance" => LevelOrderType.Relevance,
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
        string? byUserId = context.QueryString["createdBy"];
        GameUser? byUser = null;
        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        
        string? likedByUserId = context.QueryString["likedBy"];
        GameUser? likedBy = null;
        if (likedByUserId != null) likedBy = database.GetUserWithId(likedByUserId);
        
        string? queuedByUserId = context.QueryString["queuedBy"];
        GameUser? queuedBy = null;
        if (queuedByUserId != null) queuedBy = database.GetUserWithId(queuedByUserId);
        
        string? likedOrQueuedByUserId = context.QueryString["likedOrQueuedBy"];
        GameUser? likedOrQueuedBy = null;
        if (likedOrQueuedByUserId != null) likedOrQueuedBy = database.GetUserWithId(likedOrQueuedByUserId);
        
        string? completedByString = context.QueryString["completedBy"];
        GameUser? completedBy = null;
        if (completedByString != null) completedBy = database.GetUserWithId(completedByString);
        
        string? inAlbumId = context.QueryString["inAlbum"];
        GameAlbum? inAlbum = null;
        if (inAlbumId != null) inAlbum = database.GetAlbumWithId(inAlbumId);
        
        bool? inDaily = null;
        if (bool.TryParse(context.QueryString["inDaily"], out bool inDailyTemp)) inDaily = inDailyTemp;
        
        string? inDailyDateString = context.QueryString["inDailyDate"];
        long? inDailyDateLong = null;
        if (inDailyDateString != null) inDailyDateLong = long.Parse(inDailyDateString);
        DateTimeOffset? inDailyDate = null;
        if (inDailyDateLong != null) inDailyDate = DateTimeOffset.FromUnixTimeSeconds((long)inDailyDateLong);

        bool? lastDate = null;
        if (bool.TryParse(context.QueryString["inLastDaily"], out bool lastDateTemp)) lastDate = lastDateTemp;

        string? bpmString = context.QueryString["bpm"];
        int? bpm = null;
        if (bpmString != null) bpm = int.Parse(bpmString);

        string? transposeValueString = context.QueryString["transposeValue"];
        int? transposeValue = null;
        if (transposeValueString != null) transposeValue = int.Parse(transposeValueString);

        int? scaleIndex = null;
        if (int.TryParse(context.QueryString["scaleIndex"], out int scaleIndexTemp)) scaleIndex = scaleIndexTemp;

        bool? hasCar = null;
        if (bool.TryParse(context.QueryString["hasCar"], out bool hasCarTemp)) hasCar = hasCarTemp;
        
        bool? hasExplodingCar = null;
        if (bool.TryParse(context.QueryString["hasExplodingCar"], out bool hasExplodingCarTemp)) hasExplodingCar = hasExplodingCarTemp;
        
        string? searchQuery = context.QueryString["search"];
        
        string? uploadPlatformsString = context.QueryString["uploadPlatforms"];
        List<PlatformType>? uploadPlatforms = null;

        if (uploadPlatformsString != null)
        {
            uploadPlatforms = new List<PlatformType>();
            uploadPlatforms.AddRange(uploadPlatformsString.Split(",").Select(Enum.Parse<PlatformType>));
        }
        
        return new LevelFilters(byUser, likedBy, queuedBy, likedOrQueuedBy, inAlbum, inDaily, inDailyDate, lastDate, 
            searchQuery, completedBy, bpm, scaleIndex, transposeValue, hasCar, hasExplodingCar, uploadPlatforms);
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
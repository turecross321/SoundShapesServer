using Bunkum.HttpServer;
using SoundShapesServer.Database;
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

    public static LevelOrderType GetLevelOrderType(string? orderString)
    {
        return orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "totalPlays" => LevelOrderType.TotalPlays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "totalCompletions" => LevelOrderType.TotalCompletions,
            "uniqueCompletions" => LevelOrderType.UniqueCompletions,
            "likes" => LevelOrderType.Likes,
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
            _ => LevelOrderType.CreationDate
        };
    }
    
    public static LevelFilters GetLevelFilters(RequestContext context, GameDatabaseContext database)
    {
        string? byUserId = context.QueryString["byUser"];
        string? likedByUserId = context.QueryString["likedBy"];
        string? completedByString = context.QueryString["completedBy"];
        string? inAlbumId = context.QueryString["inAlbum"];
        
        bool? inDaily = null;
        if (bool.TryParse(context.QueryString["inDaily"], out bool inDailyTemp)) inDaily = inDailyTemp;
        string? inDailyDateString = context.QueryString["inDailyDate"];

        bool? lastDate = null;
        if (bool.TryParse(context.QueryString["inLastDaily"], out bool lastDateTemp)) lastDate = lastDateTemp;

        string? searchQuery = context.QueryString["search"];
        
        string? bpmString = context.QueryString["bpm"];
        string? transposeValueString = context.QueryString["transposeValue"];

        int? scaleIndex = null;
        if (int.TryParse(context.QueryString["scaleIndex"], out int scaleIndexTemp)) scaleIndex = scaleIndexTemp;

        bool? hasCar = null;
        if (bool.TryParse(context.QueryString["hasCar"], out bool hasCarTemp)) hasCar = hasCarTemp;
        
        bool? hasExplodingCar = null;
        if (bool.TryParse(context.QueryString["hasExplodingCar"], out bool hasExplodingCarTemp)) hasExplodingCar = hasExplodingCarTemp;

        GameUser? byUser = null;
        GameUser? likedBy = null;
        GameUser? completedBy = null;
        GameAlbum? inAlbum = null;
        DateTimeOffset? inDailyDate = null;
        int? bpm = null;
        int? transposeValue = null;

        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        if (likedByUserId != null) likedBy = database.GetUserWithId(likedByUserId);
        if (completedByString != null) completedBy = database.GetUserWithId(completedByString);
        if (inAlbumId != null) inAlbum = database.GetAlbumWithId(inAlbumId);
        if (inDailyDateString != null) inDailyDate = DateTimeOffset.Parse(inDailyDateString);
        if (bpmString != null) bpm = int.Parse(bpmString);
        if (transposeValueString != null) transposeValue = int.Parse(transposeValueString);

        return new LevelFilters(byUser, likedBy, inAlbum, inDaily, inDailyDate, lastDate, 
            searchQuery, completedBy, bpm, scaleIndex, transposeValue, hasCar, hasExplodingCar);
    }
    
    
}
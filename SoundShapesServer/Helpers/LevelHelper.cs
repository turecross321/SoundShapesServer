using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Levels.SSLevel;
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
    
    public static GameLevel? AnalyzeLevel(GameLevel level, byte[] levelFile)
    {
        SSLevel? ssLevel = SSLevel.FromLevelFile(levelFile);
        if (ssLevel == null)
            return null;

        GameLevel copy = level;
        copy.FileSize = levelFile.Length;
        copy.Bpm = ssLevel.Bpm;
        copy.TransposeValue = ssLevel.TransposeValue;
        copy.ScaleIndex = ssLevel.ScaleIndex;
        copy.TotalScreens = ssLevel.ScreenData.Count();
        copy.TotalEntities = ssLevel.Entities.Count() + ssLevel.EntitiesB.Count();
        copy.HasCar = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_CarCheckpoint");
        copy.HasExplodingCar = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_ExplodingCarCheckpoint");
        copy.HasUfo = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_UFOCheckpoint");
        copy.HasFirefly = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_FireflyCheckpoint");

        return copy;
    }

    private const int LevelNameCharacterLimit = 26;
    public static string AdhereToLevelNameCharacterLimit(string name)
    {
        return name[..Math.Min(name.Length, LevelNameCharacterLimit)];
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
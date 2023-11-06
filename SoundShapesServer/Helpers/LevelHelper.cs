using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    private const int LevelNameCharacterLimit = 26;

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

        if (averageAmountOfDeaths >= 2.5) return averageAmountOfDeaths / 2.5f;

        return 0;
    }

    public static string AdhereToLevelNameCharacterLimit(string name)
    {
        return name[..Math.Min(name.Length, LevelNameCharacterLimit)];
    }
}
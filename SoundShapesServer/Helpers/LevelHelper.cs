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

    public static IQueryable<GameLevel> FilterLevels(GameUser? user, IQueryable<GameLevel> levels, LevelFilters filters)
    {
        IQueryable<GameLevel> response = levels;
        if (filters.ByUser != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => Equals(l.Author, filters.ByUser))
                .AsQueryable();
        }
        if (filters.LikedByUser != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => filters.LikedByUser.LikedLevels
                    .AsEnumerable()
                    .Select(relation => relation.Level.Id)
                    .Contains(l.Id))
                .AsQueryable();
        }

        if (filters.InAlbum != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => filters.InAlbum.Levels.Contains(l))
                .AsQueryable();
        }

        if (filters.InDaily != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => l.DailyLevels.Any(dl => dl.Date == filters.InDaily.Value.Date))
                .AsQueryable();
        }

        if (filters.Search != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => l.Name.Contains(filters.Search, StringComparison.OrdinalIgnoreCase)
                            || (l.Author?.Username ?? "").Contains(filters.Search, StringComparison.OrdinalIgnoreCase)
                )
                .AsQueryable();
        }

        if (user != null && filters.Completed != null)
        {
            response = response.Where(l => l.UniqueCompletions.Contains(user));
        }

        return response;
    }

    public static float CalculateLevelDifficulty(GameLevel level)
    {
        // I know this is ugly, but this is authentic to the original servers, while also supporting decimals
        // which is used for sorting levels by difficulty.
        
        float averageAmountOfDeaths = (float)level.Deaths / level.CompletionCount;
        
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
}
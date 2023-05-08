using Realms;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
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

    public static IQueryable<GameLevel> FilterLevels(GameDatabaseContext database, IQueryable<GameLevel> levels, LevelFilters filters)
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
                GameLevel? responseLevel = response.FirstOrDefault(l => l == relation.Level);
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
            IQueryable<DailyLevel> dailyLevelObjects = database.GetDailyLevelObjects(filters.InDailyDate);

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
            GameUser? userWithSearchName = database.GetUserWithUsername(filters.Search);
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
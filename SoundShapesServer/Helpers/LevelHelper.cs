using System.Security.Cryptography;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    private const string LevelIdCharacters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int LevelIdLength = 8;
    
    public static string GenerateLevelId(RealmDatabaseContext database)
    {
        Random r = new();
        string levelId = "";
        for (int i = 0; i < LevelIdLength; i++)
        {
            levelId += LevelIdCharacters[r.Next(LevelIdCharacters.Length - 1)];
        }

        if (database.GetLevelWithId(levelId) == null) return levelId; // Return if LevelId has not been used before
        return GenerateLevelId(database); // Generate new LevelId if it already exists
    }

    private static IQueryable<GameLevel> RandomizeLevelOrder(IQueryable<GameLevel> levels)
    {
        if (levels == null) throw new ArgumentNullException(nameof(levels));
        DateTime seedDateTime = DateTime.Today;
        byte[] seedBytes = BitConverter.GetBytes(seedDateTime.Ticks);
        byte[] hashBytes = MD5.HashData(seedBytes);
        int seed = BitConverter.ToInt32(hashBytes, 0);

        Random rng = new(seed);

        return levels.AsEnumerable()
            .OrderBy(_ => rng.Next())
            .AsQueryable();
    }
    public static IQueryable<GameLevel> OrderLevels(IQueryable<GameLevel> levels, LevelOrderType orderType)
    {
        return orderType switch
        {
            LevelOrderType.CreationDate => levels.AsEnumerable().OrderBy(l => l.CreationDate).AsQueryable(),
            LevelOrderType.ModificationDate => levels.AsEnumerable().OrderBy(l => l.ModificationDate).AsQueryable(),
            LevelOrderType.Plays => levels.AsEnumerable().OrderBy(l => l.Plays).AsQueryable(),
            LevelOrderType.UniquePlays => levels.AsEnumerable().OrderBy(l => l.UniquePlays.Count).AsQueryable(),
            LevelOrderType.FileSize => levels.AsEnumerable().OrderBy(l => l.FileSize).AsQueryable(),
            LevelOrderType.Difficulty => levels.AsEnumerable().OrderBy(l => l.Difficulty).AsQueryable(),
            LevelOrderType.Relevance => levels.AsEnumerable()
                .OrderBy(l=> l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5)
                .AsQueryable(),
            LevelOrderType.Random => RandomizeLevelOrder(levels),
            LevelOrderType.DoNotOrder => levels,
            _ => levels
        };
    }
}
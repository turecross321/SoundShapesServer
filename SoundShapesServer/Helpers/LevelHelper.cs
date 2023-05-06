using System.Security.Cryptography;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

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

    public static IQueryable<GameLevel>? FilterLevels(RealmDatabaseContext database, GameUser? user, IQueryable<GameLevel> levels, string? byUser, string? likedByUser, string? inAlbum, string? inDaily, bool? completed)
    {
        IQueryable<GameLevel> response = levels;
        if (byUser != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => l.Author?.Id == byUser)
                .AsQueryable();
        }
        if (likedByUser != null)
        {
            GameUser? userToGetLevelsFrom = database.GetUserWithId(likedByUser);
            if (userToGetLevelsFrom == null) return null;

            response = response
                .AsEnumerable()
                .Where(l => userToGetLevelsFrom.LikedLevels
                    .Select(relation => relation.Level.Id)
                    .Contains(l.Id))
                .AsQueryable();
        }

        if (inAlbum != null)
        {
            GameAlbum? albumToGetLevelsFrom = database.GetAlbumWithId(inAlbum);
            if (albumToGetLevelsFrom == null) return null;

            response = response
                .AsEnumerable()
                .Where(l => albumToGetLevelsFrom.Levels.Contains(l))
                .AsQueryable();
        }

        if (inDaily != null)
        {
            DateTimeOffset date = DateTimeOffset.Parse(inDaily);
            IQueryable<DailyLevel> dailyLevelObjects = database.GetDailyLevelObjects(date);

            response = response.AsEnumerable().Where(l => dailyLevelObjects.Select(d => d.Level).Contains(l)).AsQueryable();
        }

        if (user != null && completed != null)
        {
            response = response.Where(l => l.UsersWhoHaveCompletedLevel.Contains(user));
        }

        return response;
    }

    public static IQueryable<GameLevel> OrderLevels(IQueryable<GameLevel> levels, LevelOrderType orderType, bool descending)
    {
        IQueryable<GameLevel> response = levels;

        response = orderType switch
        {
            LevelOrderType.CreationDate => response.OrderBy(l => l.CreationDate).AsQueryable(),
            LevelOrderType.ModificationDate => response.OrderBy(l => l.ModificationDate).AsQueryable(),
            LevelOrderType.Plays => response.OrderBy(l => l.Plays).AsQueryable(),
            LevelOrderType.UniquePlays => response.OrderBy(l => l.UniquePlays.Count).AsQueryable(),
            LevelOrderType.FileSize => response.OrderBy(l => l.FileSize).AsQueryable(),
            LevelOrderType.Difficulty => response.OrderBy(l => l.Difficulty).AsQueryable(),
            LevelOrderType.Relevance => response
                .OrderBy(l=> l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5)
                .AsQueryable(),
            LevelOrderType.Random => RandomizeLevelOrder(response.AsQueryable()),
            LevelOrderType.DoNotOrder => response.AsQueryable(),
            _ => OrderLevels(response, LevelOrderType.CreationDate, descending)
        };

        if (descending) response = response.AsEnumerable().Reverse().AsQueryable();

        return response;
    }
}
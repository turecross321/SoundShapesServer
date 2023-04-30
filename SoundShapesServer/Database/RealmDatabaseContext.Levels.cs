using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using Bunkum.HttpServer.Storage;
using Realms;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameLevel PublishLevel(LevelPublishRequest request, GameUser user)
    {
        string levelId = request.Id;
            
        GameLevel level = new(levelId, user, request.Title, request.Language, request.FileSize, request.Modified);

        _realm.Write(() =>
        {
            _realm.Add(level);
        });

        return level;
    }

    public GameLevel? UpdateLevel(LevelPublishRequest updatedLevel, GameLevel level, GameUser user)
    {
        if (user.Id  != level.Author.Id) return null;
        
        _realm.Write(() =>
        {
            level.Name = updatedLevel.Title;
            level.Language = updatedLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
            level.FileSize = updatedLevel.FileSize;
        });

        return level;
    }
    
    // Not database related, but idk where this should be otherwise.
    private static void RemoveLevelResources(GameLevel level, IDataStore dataStore)
    {
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Image));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Level));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Sound));
    }
    
    public void RemoveLevel(GameLevel level, IDataStore dataStore)
    {
        RemoveLevelResources(level, dataStore);

        RemoveAllReportsWithContentId(level.Id);
        
        _realm.Write(() =>
        {
            foreach (GameAlbum album in level.Albums)
            {
                album.Levels.Remove(level);
            }
            _realm.RemoveRange(level.DailyLevels);
            _realm.RemoveRange(level.Likes);
            _realm.RemoveRange(_realm.All<LeaderboardEntry>().Where(e=>e.LevelId == level.Id));
            _realm.Remove(level);
        });
    }
    
    public GameLevel? GetLevelWithId(string id) => _realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);
    public IQueryable<GameLevel> GetLevelsPublishedByUser(GameUser user)
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .Where(l => l.Author.Id == user.Id);

        return entries.AsQueryable();
    }

    public IQueryable<GameLevel> SearchForLevels(string query)
    {
        string[] keywords = query.Split(' ');
        if (keywords.Length == 0) return Enumerable.Empty<GameLevel>().AsQueryable();
        
        IQueryable<GameLevel>? entries = _realm.All<GameLevel>();
        
        foreach (string keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword)) continue;

            entries = entries.Where(l =>
                l.Name.Like(keyword, false)
            );
        }

        return entries;
    }

    public IQueryable<GameLevel> DailyLevels(DateTimeOffset date)
    {
        List<DailyLevel> entries = DailyLevelObjects(date).ToList();
        
        List<GameLevel> levels = entries.Where(dailyLevel=>dailyLevel.Level != null).Select(l =>
        {
            Debug.Assert(l.Level != null, "l.Level != null");
            return l.Level;
        }).ToList();

        return levels.AsQueryable();
    }

    public IQueryable<GameLevel> RandomLevels()
    {
        DateTime seedDateTime = DateTime.Today;
        byte[] seedBytes = BitConverter.GetBytes(seedDateTime.Ticks);
        byte[] hashBytes = MD5.Create().ComputeHash(seedBytes);
        int seed = BitConverter.ToInt32(hashBytes, 0);

        Random rng = new(seed);

        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderBy(_ => rng.Next());

        return entries.AsQueryable();
    }
    public IQueryable<GameLevel> GreatestHits()
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l => l.UniquePlays.Count() * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5);

        return entries.AsQueryable();
    }

    public IQueryable<GameLevel> NewestLevels()
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.CreationDate);

        return entries.AsQueryable();
    }
    public IQueryable<GameLevel> TopLevels()
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.UniquePlays.Count);

        return entries.AsQueryable();
    }

    public IQueryable<GameLevel> LargestLevels()
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.FileSize);

        return entries.AsQueryable();
    }
    
    public IQueryable<GameLevel> HardestLevels()
    {
        IEnumerable<GameLevel> entries = _realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.Deaths / l.UniquePlays.Count);

        return entries.AsQueryable();
    }

    public void AddUserToLevelCompletions(GameLevel level, GameUser user)
    {
        if (level.UsersWhoHaveCompletedLevel.Contains(user)) return;

        _realm.Write(() =>
        {
            level.UsersWhoHaveCompletedLevel.Add(user);
        });
    }
    public void AddCompletionToLevel(GameLevel level)
    {
        _realm.Write(() =>
        {
            level.CompletionCount++;
        });
    }
    public void AddPlayToLevel(GameLevel level)
    {
        _realm.Write(() =>
        {
            level.Plays++;
        });
    }
    public void AddUniquePlayToLevel(GameLevel level, GameUser user)
    {
        if (level.UniquePlays.Contains(user)) return;
        
        _realm.Write(() =>
        {
            level.UniquePlays.Add(user);
        });
    }

    public void AddDeathsToLevel(GameLevel level, int deaths)
    {
        _realm.Write(() =>
        {
            level.Deaths += deaths;
        });
    }
}
using System.Diagnostics;
using Bunkum.HttpServer.Storage;
using Realms;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameLevel CreateLevel(PublishLevelRequest request, GameUser user)
    {
        string levelId = request.Id;
        GameLevel level = new(levelId, user, request.Name, request.Language, request.FileSize, request.Modified);

        _realm.Write(() =>
        {
            _realm.Add(level);
        });

        CreateEvent(user, EventType.Publish, null, level);
        
        return level;
    }

    public GameLevel EditLevel(PublishLevelRequest updatedPublishLevel, GameLevel level)
    {
        _realm.Write(() =>
        {
            level.Name = updatedPublishLevel.Name;
            level.Language = updatedPublishLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
        });

        return level;
    }

    public void SetLevelFileSize(GameLevel level, long fileSize)
    {
        _realm.Write(() =>
        {
            level.FileSize = fileSize;
        });
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

    private IQueryable<GameLevel> GetLevelsWithIds(IEnumerable<string> ids)
    {
        List<GameLevel> levels = new ();
        
        foreach (string levelId in ids) 
        {
            GameLevel? level = GetLevelWithId(levelId);
            if (level != null) levels.Add(level);
        }

        return levels.AsQueryable();
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
                l.Author != null && (l.Name.Like(keyword, false) || l.Author.Username.Like(keyword, false))
            );
        }

        return entries;
    }

    public IQueryable<GameLevel> GetDailyLevels(DateTimeOffset date, bool getOldLevelsIfThereAreNone = false)
    {
        List<DailyLevel> entries = GetDailyLevelObjects(date, getOldLevelsIfThereAreNone).ToList();
        
        List<GameLevel> levels = entries.Select(l =>
        {
            Debug.Assert(l.Level != null, "l.Level != null");
            return l.Level;
        }).ToList();

        return levels.AsQueryable();
    }

    public IQueryable<GameLevel> GetLevels()
    {
        return _realm.All<GameLevel>();
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

    public void SetLevelDifficulty(GameLevel level)
    {
        _realm.Refresh();

        _realm.Write(() =>
        {
            level.Difficulty = LevelHelper.CalculateLevelDifficulty(level);
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
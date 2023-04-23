using System.Security.Cryptography;
using Realms;
using Realms.Sync;
using SoundShapesServer.Configuration;
using SoundShapesServer.Requests;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LevelHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public LevelPublishResponse PublishLevel(LevelPublishRequest request, GameUser user)
    {
        string levelId = request.Id;
            
        GameLevel level = new GameLevel
        {
            Id = levelId,
            Author = user,
            Name = request.Title,
            Language = request.Language,
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow,
            FileSize = request.FileSize
        };

        this._realm.Write(() =>
        {
            this._realm.Add(level);
        });

        return GeneratePublishResponse(level);
    }

    public LevelPublishResponse? UpdateLevel(LevelPublishRequest updatedLevel, GameLevel level, GameUser user)
    {
        if (!user.Equals(level.Author)) return null;
        
        this._realm.Write(() =>
        {
            level.Name = updatedLevel.Title;
            level.Language = updatedLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
            level.FileSize = updatedLevel.FileSize;
        });

        return GeneratePublishResponse(level);
    }
    
    public bool UnPublishLevel(GameLevel level)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(level);
        });

        return true;
    }
    
    public GameLevel? GetLevelWithId(string id) => this._realm.All<GameLevel>().FirstOrDefault(l => l.Id == id);
    public IQueryable<GameLevel> GetLevelsPublishedByUser(GameUser user)
    {
        IQueryable<GameLevel> entries = this._realm.All<GameLevel>()
            .Where(l => l.Author == user);

        return entries;
    }

    public IQueryable<GameLevel> SearchForLevels(string query)
    {
        string[] keywords = query.Split(' ');
        if (keywords.Length == 0) return null;
        
        IQueryable<GameLevel> entries = this._realm.All<GameLevel>();
        
        foreach (string keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword)) continue;

            entries = entries.Where(l =>
                l.Name.Like(keyword, false)
            );
        }

        return entries;
    }

    public IQueryable<GameLevel> DailyLevels()
    {
        List<DailyLevel> entries = this._realm.All<DailyLevel>()
            .OrderByDescending(l=>l.Date)
            .ToList();
        
        List<GameLevel> levels = new List<GameLevel>();

        for (int i = 0; i < entries.Count; i++)
        {
            levels.Add(entries[i].Level);
        }

        return levels.AsQueryable();
    }

    public IQueryable<GameLevel> RandomLevels()
    {
        DateTime seedDateTime = DateTime.Today;
        byte[] seedBytes = BitConverter.GetBytes(seedDateTime.Ticks);
        byte[] hashBytes = MD5.Create().ComputeHash(seedBytes);
        int seed = BitConverter.ToInt32(hashBytes, 0);

        Random rng = new Random(seed);

        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderBy(level => rng.Next());

        return entries.AsQueryable();
    }
    public IQueryable<GameLevel> GreatestHits()
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l => l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5);

        return entries.AsQueryable();
    }

    public IQueryable<GameLevel> NewestLevels()
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.CreationDate);

        return entries.AsQueryable();
    }
    public IQueryable<GameLevel> TopLevels()
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.UniquePlays.Count);

        return entries.AsQueryable();
    }

    public IQueryable<GameLevel> LargestLevels()
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.FileSize);

        return entries.AsQueryable();
    }
    
    public IQueryable<GameLevel> HardestLevels()
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.Deaths / l.UniquePlays.Count);

        return entries.AsQueryable();
    }

    public void AddUserToLevelCompletions(GameLevel level, GameUser user)
    {
        if (level.UsersWhoHaveCompletedLevel.Contains(user)) return;

        this._realm.Write((() =>
        {
            level.UsersWhoHaveCompletedLevel.Add(user);
        }));
    }
    public void AddCompletionToLevel(GameLevel level)
    {
        this._realm.Write((() =>
        {
            level.CompletionCount++;
        }));
    }
    public void AddPlayToLevel(GameLevel level)
    {
        this._realm.Write((() =>
        {
            level.Plays++;
        }));
    }
    public void AddUniquePlayToLevel(GameLevel level, GameUser user)
    {
        if (level.UniquePlays.Contains(user)) return;
        
        this._realm.Write((() =>
        {
            level.UniquePlays.Add(user);
        }));
    }

    public void AddDeathsToLevel(GameLevel level, int deaths)
    {
        this._realm.Write((() =>
        {
            level.Deaths += deaths;
        }));
    }
}
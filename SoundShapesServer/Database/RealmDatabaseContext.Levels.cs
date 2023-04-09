using Realms;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Levels;
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
            Description = request.Description,
            Language = request.Language,
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow
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
            level.Description = updatedLevel.Description;
            level.Language = updatedLevel.Language;
            level.ModificationDate = DateTimeOffset.UtcNow;
        });

        return GeneratePublishResponse(level);
    }

    public void SetUserFeaturedLevel(GameUser user, GameLevel level)
    {
        this._realm.Write((() =>
        {
            user.FeaturedLevel = level;
        }));
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
    public LevelsWrapper GetLevelsPublishedByUser(GameUser user, GameUser userToGetLevelsFrom, int from, int count)
    {
        IQueryable<GameLevel> entries = this._realm.All<GameLevel>()
            .Where(l => l.Author == userToGetLevelsFrom);

        int totalEntries = entries.Count();

        GameLevel[] selectedEntries = entries
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
    }

    public LevelsWrapper? SearchForLevels(GameUser user, string query, int from, int count)
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

        int totalEntries = entries.Count();
        
        GameLevel[] selectedEntries = entries
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
    }

    public LevelsWrapper DailyLevels(GameUser user, int from, int count)
    {
        List<DailyLevel> entries = this._realm.All<DailyLevel>()
            .OrderByDescending(l=>l.Date)
            .ToList();

        int totalEntries = entries.Count;

        DailyLevel[] dailyLevelEntries = entries
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        GameLevel[] levels = new GameLevel[dailyLevelEntries.Length];

        for (int i = 0; i < dailyLevelEntries.Length; i++)
        {
            levels[i] = dailyLevelEntries[i].Level;
        }

        return LevelsToLevelsWrapper(levels, user, totalEntries, from, count);
    }
    public LevelsWrapper GreatestHits(GameUser user, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l => l.UniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.CreationDate).TotalDays * 0.5);

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
    }

    public LevelsWrapper NewestLevels(GameUser user, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.CreationDate);

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
    }
    public LevelsWrapper TopLevels(GameUser user, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l=>l.UniquePlays.Count);

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
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
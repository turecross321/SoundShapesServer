using Realms;
using SoundShapesServer.Helpers;
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
        string levelId = request.levelId;
            
        GameLevel level = new GameLevel
        {
            id = levelId,
            author = user,
            title = request.title,
            description = request.description,
            scp_np_language = request.sce_np_language,
            created = DateTimeOffset.UtcNow,
            modified = DateTimeOffset.UtcNow
        };

        this._realm.Write(() =>
        {
            this._realm.Add(level);
        });

        return GeneratePublishResponse(level);
    }

    public LevelPublishResponse? UpdateLevel(LevelPublishRequest updatedLevel, GameLevel level, GameUser user)
    {
        if (!user.Equals(level.author)) return null;
        
        this._realm.Write(() =>
        {
            level.title = updatedLevel.title;
            level.description = updatedLevel.description;
            level.scp_np_language = updatedLevel.sce_np_language;
            level.modified = DateTimeOffset.UtcNow;
        });

        return GeneratePublishResponse(level);
    }

    public void SetFeaturedLevel(GameUser user, GameLevel level)
    {
        this._realm.Write((() =>
        {
            user.featuredLevel = level;
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
    
    public GameLevel? GetLevelWithId(string id) => this._realm.All<GameLevel>().FirstOrDefault(l => l.id == id);
    public LevelsWrapper GetLevelsPublishedByUser(GameUser user, GameUser userToGetLevelsFrom, int from, int count)
    {
        IQueryable<GameLevel> entries = this._realm.All<GameLevel>()
            .Where(l => l.author == userToGetLevelsFrom);

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
                l.title.Like(keyword, false)
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
            .OrderByDescending(l=>l.date)
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
            levels[i] = dailyLevelEntries[i].level;
        }

        return LevelsToLevelsWrapper(levels, user, totalEntries, from, count);
    }
    public LevelsWrapper GreatestHits(GameUser user, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable()
            .OrderByDescending(l => l.uniquePlays.Count * 0.5 + (DateTimeOffset.UtcNow - l.created).TotalDays * 0.5);

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
            .OrderByDescending(l=>l.created);

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
            .OrderByDescending(l=>l.uniquePlays.Count);

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return LevelsToLevelsWrapper(selectedEntries, user, totalEntries, from, count);
    }

    public void AddCompletionistToLevel(GameLevel level, GameUser user)
    {
        if (level.completionists.Contains(user)) return;
        
        this._realm.Write((() =>
        {
            level.completionists.Add(user);
        }));
    }
    public void AddCompletionToLevel(GameLevel level)
    {
        this._realm.Write((() =>
        {
            level.completions++;
        }));
    }
    public void AddPlayToLevel(GameLevel level)
    {
        this._realm.Write((() =>
        {
            level.plays++;
        }));
    }
    public void AddUniquePlayToLevel(GameLevel level, GameUser user)
    {
        if (level.uniquePlays.Contains(user)) return;
        
        this._realm.Write((() =>
        {
            level.uniquePlays.Add(user);
        }));
    }

    public void AddDeathsToLevel(GameLevel level, int deaths)
    {
        this._realm.Write((() =>
        {
            level.deaths += deaths;
        }));
    }
}
using Realms;
using Realms.Sync;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

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
            visibility = Visibility.EVERYONE.ToString(),
            scp_np_language = request.sce_np_language,
            creationTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            metadata = GenerateMetadata(request),
        };

        this._realm.Write(() =>
        {
            this._realm.Add(level);
        });

        return GeneratePublishResponse(level);
    }

    public LevelPublishResponse UpdateLevel(LevelPublishRequest updatedLevel, GameLevel level, GameUser user)
    {
        if (!user.Equals(level.author)) return null;
        
        this._realm.Write(() =>
        {
            level.title = updatedLevel.title;
            level.description = updatedLevel.description;
            level.scp_np_language = updatedLevel.sce_np_language;
            level.creationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            
            level.metadata.displayName = updatedLevel.title;
            level.metadata.image = "";
            level.metadata.modified = DateTimeOffset.UtcNow;
            level.metadata.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            level.metadata.sce_np_language = updatedLevel.sce_np_language;
        });

        return GeneratePublishResponse(level);
    }

    private const string levelIdCharacters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int levelIdLength = 8;
    
    public string GenerateLevelId()
    {
        Random r = new Random();
        string levelId = "";
        for (int i = 0; i < levelIdLength; i++)
        {
            levelId += levelIdCharacters[r.Next(levelIdCharacters.Length - 1)];
        }

        if (GetLevelWithId(levelId) == null) return levelId; // Return if LevelId has not been used before
        return GenerateLevelId(); // Generate new LevelId if it already exists
    }
    
    public static LevelMetadata GenerateMetadata(LevelPublishRequest level)
    {
        LevelMetadata metadata = new LevelMetadata();

        metadata.displayName = level.title;
        metadata.image = "";
        metadata.created = DateTimeOffset.UtcNow;
        metadata.modified = DateTimeOffset.UtcNow;
        metadata.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        metadata.sce_np_language = level.sce_np_language;

        return metadata;
    }

    private LevelPublishResponse GeneratePublishResponse(GameLevel level)
    {
        return new () {
            id = IdFormatter.FormatLevelPublishId(level.id, level.creationTime),
            type = ResponseType.upload.ToString(),
            author = new()
            {
                id = IdFormatter.FormatUserId(level.author.id),
                type = ResponseType.identity.ToString(),
                displayName = level.author.display_name
            },
            title = level.title,
            dependencies = new List<string>(),
            visibility = level.visibility,
            description = level.description,
            extraData = new ExtraData() { sce_np_language = level.scp_np_language },
            parent = new()
            {
                id = IdFormatter.FormatLevelId(level.id),
                type = ResponseType.level.ToString()
            },
            creationTime = level.creationTime
        };   
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
    public IEnumerable<GameLevel> GetAllLevels()
    {
        return this._realm.All<GameLevel>().AsEnumerable();
    }
    public IEnumerable<GameLevel> GetLevelsPublishedByUser(GameUser user)
    {
        return this._realm.All<GameLevel>().Where(l => l.author == user).AsEnumerable();
    }

    public IEnumerable<GameLevel> SearchForLevels(string query)
    {
        string[] keywords = query.Split(' ');
        if (keywords.Length == 0) return new List<GameLevel>();
        
        IQueryable<GameLevel> levels = this._realm.All<GameLevel>();
        
        foreach (string keyword in keywords)
        {
            if(string.IsNullOrWhiteSpace(keyword)) continue;

            levels = levels.Where(l =>
                l.title.Like(keyword, false)
            );
        }

        return levels.AsEnumerable();
    }
}
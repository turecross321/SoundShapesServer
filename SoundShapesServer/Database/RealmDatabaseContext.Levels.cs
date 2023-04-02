using Realms.Sync;
using SoundShapesServer.Enums;
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
            
        GameLevel gameLevel = new GameLevel
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
            this._realm.Add(gameLevel);
        });

        LevelPublishResponse publishResponse = new()
        {
            id = FormatLevelPublishId(gameLevel.id, gameLevel.creationTime),
            type = ResponseType.upload.ToString(),
            author = new()
            {
                id = FormatUserId(gameLevel.author.id),
                type = ResponseType.identity.ToString(),
                display_name = gameLevel.author.display_name
            },
            title = gameLevel.title,
            dependencies = new List<string>(),
            visibility = gameLevel.visibility,
            description = gameLevel.description,
            extraData = new ExtraData() { sce_np_language = gameLevel.scp_np_language },
            parent = new()
            {
                id = FormatLevelId(gameLevel.id),
                type = ResponseType.level.ToString()
            },
            creationTime = gameLevel.creationTime
        };
        
        return publishResponse;
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
    public string FormatLevelId(string id)
    {
        return $"/~level:{id}";
    }
    private static string FormatLevelPublishId(string id, long creationTime)
    {
        return $"/~level:{id}/~upload:{creationTime}";
    }
    public static LevelMetadata GenerateMetadata(LevelPublishRequest level)
    {
        LevelMetadata metadata = new()
        {
            displayName = level.title,
            image = "",
            created = DateTimeOffset.Now,
            modified = DateTimeOffset.UtcNow,
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            sce_np_language = level.sce_np_language
        };

        return metadata;
    }

    public GameLevel? GetLevelWithId(string id) => this._realm.All<GameLevel>().FirstOrDefault(l => l.id == id);
    public List<GameLevel> GetAllLevels()
    {
        return this._realm.All<GameLevel>().ToList();
    }
    public List<GameLevel> GetLevelsPublishedByUser(GameUser user, int count)
    {
        return this._realm.All<GameLevel>().Where(l => l.author == user).AsEnumerable().Take(count).ToList();
    }
}
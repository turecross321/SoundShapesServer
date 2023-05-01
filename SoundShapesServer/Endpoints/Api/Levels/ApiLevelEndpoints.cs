using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    public ApiLevelResponseWrapper? Levels(RequestContext context, RealmDatabaseContext database, GameUser? user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");

        string? orderString = context.QueryString["orderBy"];
        string category = context.QueryString["category"] ?? "all";

        IQueryable<GameLevel>? levels = null;
        
        switch (category)
        {
            case "byUser":
            {
                string? userId = context.QueryString["userId"];
                if (userId == null) return null;

                GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
                if (userToGetLevelsFrom == null) return null;

                levels = userToGetLevelsFrom.Levels;
                break;
            }
            case "likedByUser":
            {
                string? userId = context.QueryString["userId"];
                if (userId == null) return null;

                GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
                if (userToGetLevelsFrom == null) return null;

                levels = userToGetLevelsFrom.LikedLevels.Select(r=>r.Level);
                break;
            }
            case "daily":
                levels = database.GetDailyLevels(DateTimeOffset.UtcNow);
                break;
        }

        levels ??= database.GetLevels();

        LevelOrderType order = orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "plays" => LevelOrderType.Plays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "relevance" => LevelOrderType.Relevance,
            _ => LevelOrderType.CreationDate
        };
        
        return new ApiLevelResponseWrapper(levels, from, count, user, order, descending);
    }

    [ApiEndpoint("level/{levelId}")]
    [Authentication(false)]
    public ApiLevelFullResponse? Level(RequestContext context, RealmDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level, user);
    }
}
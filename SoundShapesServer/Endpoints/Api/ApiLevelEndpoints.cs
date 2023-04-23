using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("user/{authorId}/levels")]
    [Authentication(false)]
    public ApiLevelResponseWrapper? LevelsByUser(RequestContext context, RealmDatabaseContext database, string authorId, GameUser? user)
    {
        GameUser? author = database.GetUserWithId(authorId);
        if (author == null) return null;
        
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        IQueryable<GameLevel> levels = database.GetLevelsPublishedByUser(author);

        return LevelHelper.LevelsToApiLevelResponseWrapper(levels, from, count, user);
    }

    [ApiEndpoint("level/{levelId}")]
    [Authentication(false)]
    public ApiLevelResponse? Level(RequestContext context, RealmDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        return LevelHelper.LevelToApiLevelResponse(level, user);
    }
}
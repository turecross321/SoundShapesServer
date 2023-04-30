using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

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

        return new ApiLevelResponseWrapper(levels, from, count, user);
    }

    [ApiEndpoint("level/{levelId}")]
    [Authentication(false)]
    public ApiLevelResponse? Level(RequestContext context, RealmDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelResponse(level, user);
    }
}
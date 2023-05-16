using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelInteractionEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{id}/liked", ContentType.Json)]
    public ApiIsLevelLikedResponse? CheckIfUserHasLikedLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return null;

        return new ApiIsLevelLikedResponse()
        {
            IsLiked = database.IsUserLikingLevel(user, level)
        };
    }

    [ApiEndpoint("levels/id/{id}/like", Method.Post)]
    public Response LikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.LikeLevel(user, level)) return HttpStatusCode.Created;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("levels/id/{id}/unLike", Method.Post)]
    public Response UnLikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.UnLikeLevel(user, level)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelInteractionEndpoints : EndpointGroup
{
    [ApiEndpoint("level/{id}/liked", ContentType.Json)]
    public ApiIsLevelLikedResponse? CheckIfUserHasLikedLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return null;

        return new ApiIsLevelLikedResponse()
        {
            IsLiked = database.IsUserLikingLevel(user, level)
        };
    }

    [ApiEndpoint("level/{id}/like", Method.Post)]
    public Response LikeLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.LikeLevel(user, level)) return HttpStatusCode.Created;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("level/{id}/unLike", Method.Post)]
    public Response UnLikeLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.UnLikeLevel(user, level)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
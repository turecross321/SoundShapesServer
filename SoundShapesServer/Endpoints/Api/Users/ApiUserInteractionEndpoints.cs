using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiUserInteractionEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}/followed")]
    public ApiIsUserFollowedResponse? CheckIfUserIsFollowed(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;

        return new ApiIsUserFollowedResponse
        {
            IsFollowed = database.IsUserFollowingOtherUser(user, recipient)
        };
    }

    [ApiEndpoint("user/{id}/follow", Method.Post)]
    public Response FollowUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Id == user.Id) return HttpStatusCode.Forbidden;

        if (database.FollowUser(user, recipient)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("user/{id}/unFollow", Method.Post)]
    public Response UnFollowUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Id == user.Id) return HttpStatusCode.Forbidden;

        if (database.UnFollowUser(user, recipient)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
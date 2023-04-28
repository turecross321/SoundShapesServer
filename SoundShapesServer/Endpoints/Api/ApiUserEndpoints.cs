using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Services;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}")]
    [Authentication(false)]
    public ApiUserResponse? GetUser(RequestContext context, RealmDatabaseContext database, string id, GameUser? user)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        if (userToCheck == null) return null;

        bool? following = null;
        if (user != null)
        {
            following = database.IsUserFollowingOtherUser(user, userToCheck);
        }

        return UserHelper.UserToApiUserResponse(userToCheck, following);
    }

    [ApiEndpoint("user/{id}/followed")]
    public ApiIsUserFollowedResponse? CheckIfUserIsFollowed(RequestContext context, RealmDatabaseContext database, string id, GameUser user)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;

        return new ApiIsUserFollowedResponse
        {
            IsFollowed = database.IsUserFollowingOtherUser(user, recipient)
        };
    }

    [ApiEndpoint("user/{id}/follow", Method.Post)]
    public Response FollowUser(RequestContext context, RealmDatabaseContext database, string id, GameUser user)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Equals(user)) return HttpStatusCode.Forbidden;

        if (database.FollowUser(user, recipient)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("user/{id}/unFollow", Method.Post)]
    public Response UnFollowUser(RequestContext context, RealmDatabaseContext database, string id, GameUser user)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Equals(user)) return HttpStatusCode.Forbidden;

        if (database.UnFollowUser(user, recipient)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
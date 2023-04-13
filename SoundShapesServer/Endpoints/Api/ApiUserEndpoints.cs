using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}")]
    [Authentication(false)]
    public ApiUserResponse? GetUser(RequestContext context, RealmDatabaseContext database, string id)
    {
        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        return UserHelper.UserToApiUserResponse(user);
    }

    [ApiEndpoint("user/{id}/followed")]
    public APiIsUserFollowedResponse? CheckIfUserIsFollowed(RequestContext context, RealmDatabaseContext database, string id, GameUser user)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;

        return new APiIsUserFollowedResponse
        {
            IsFollowed = database.IsUserFollowingOtherUser(user, recipient)
        };
    }
}
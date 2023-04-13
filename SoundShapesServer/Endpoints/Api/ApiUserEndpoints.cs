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
    public ApiUserResponse? GetUser(RequestContext context, RealmDatabaseContext database, string id, GameUser? user)
    {
        GameUser? userToGet = database.GetUserWithId(id);
        if (userToGet == null) return null;

        bool followedByYou = false;
        
        if (user != null) followedByYou = database.IsUserFollowingOtherUser(user, userToGet);
        
        return UserHelper.UserToApiUserResponse(userToGet, followedByYou);
    }
}
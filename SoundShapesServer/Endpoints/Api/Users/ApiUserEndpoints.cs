using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Users;
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
}
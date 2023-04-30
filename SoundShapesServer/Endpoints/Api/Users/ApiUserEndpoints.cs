using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}")]
    [Authentication(false)]
    public ApiUserResponse? GetUser(RequestContext context, RealmDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserResponse(userToCheck);
    }
}
using System.Net;
using System.Reflection.Metadata.Ecma335;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiManageUserEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}/remove")]
    public Response RemoveUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToRemove = database.GetUserWithId(id);
        if (userToRemove == null) return HttpStatusCode.NotFound;
        
        database.RemoveUser(userToRemove);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("user/{id}/setPermissions")]
    public Response SetUserPermissions(RequestContext context, RealmDatabaseContext database, GameUser user, string id, ApiSetUserPermissionsRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToSetPermissionsOf = database.GetUserWithId(id);
        if (userToSetPermissionsOf == null) return HttpStatusCode.Forbidden;

        if (Enum.TryParse(body.PermissionsType.ToString(), out PermissionsType type) == false) return HttpStatusCode.BadRequest;
        if (body.PermissionsType > user.PermissionsType || userToSetPermissionsOf.PermissionsType >= user.PermissionsType) return HttpStatusCode.Unauthorized;
        
        
        database.SetUserPermissions(user, type);
        return HttpStatusCode.OK;
    }
}
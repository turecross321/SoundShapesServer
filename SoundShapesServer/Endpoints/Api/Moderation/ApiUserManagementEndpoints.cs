using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiUserManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}/remove", Method.Post)]
    public Response RemoveUser(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToRemove = database.GetUserWithId(id);
        if (userToRemove == null) return HttpStatusCode.NotFound;

        if (userToRemove.PermissionsType >= user.PermissionsType) return HttpStatusCode.Unauthorized;
        
        database.RemoveUser(userToRemove, dataStore);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("users/id/{id}/setPermissions", Method.Post)]
    public Response SetUserPermissions(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiSetUserPermissionsRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToSetPermissionsOf = database.GetUserWithId(id);
        if (userToSetPermissionsOf == null) return HttpStatusCode.Forbidden;

        if (Enum.TryParse(body.PermissionsType.ToString(), out PermissionsType type) == false) return HttpStatusCode.BadRequest;
        if (body.PermissionsType > user.PermissionsType || userToSetPermissionsOf.PermissionsType >= user.PermissionsType) return HttpStatusCode.Unauthorized;
        
        
        database.SetUserPermissions(userToSetPermissionsOf, type);
        return HttpStatusCode.Created;
    }
}
using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes user with specified ID.")]
    public Response RemoveUser(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameUser? userToRemove = database.GetUserWithId(id);
        if (userToRemove == null) return HttpStatusCode.NotFound;

        if (userToRemove.PermissionsType >= user.PermissionsType) return HttpStatusCode.Unauthorized;
        
        database.RemoveUser(userToRemove, dataStore);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("users/id/{id}/setPermissions", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the permissions of user with specified ID.")]
    public Response SetUserPermissions(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiSetUserPermissionsRequest body)
    {
        GameUser? userToSetPermissionsOf = database.GetUserWithId(id);
        if (userToSetPermissionsOf == null) return HttpStatusCode.Forbidden;

        if (Enum.TryParse(body.PermissionsType.ToString(), out PermissionsType type) == false) 
            return HttpStatusCode.BadRequest;
        if (body.PermissionsType > user.PermissionsType || userToSetPermissionsOf.PermissionsType >= user.PermissionsType) 
            return HttpStatusCode.Unauthorized;
        
        
        database.SetUserPermissions(userToSetPermissionsOf, type);
        return HttpStatusCode.Created;
    }
}
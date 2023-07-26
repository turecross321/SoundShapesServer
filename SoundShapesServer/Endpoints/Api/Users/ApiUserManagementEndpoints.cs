using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes user with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.UserNotFoundWhen)]
    [DocError(typeof(UnauthorizedError), UnauthorizedError.NoDeletionPermissionWhen)]
    public Response RemoveUser(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameUser? userToRemove = database.GetUserWithId(id);
        if (userToRemove == null) return HttpStatusCode.NotFound;

        if (userToRemove.PermissionsType >= user.PermissionsType) 
            return new Response(UnauthorizedError.NoDeletionPermissionWhen, ContentType.Plaintext, HttpStatusCode.Unauthorized);
        
        database.RemoveUser(userToRemove, dataStore);
        return HttpStatusCode.NoContent;
    }

    [ApiEndpoint("users/id/{id}/setPermissions", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the permissions of user with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.UserNotFoundWhen)]
    [DocError(typeof(UnauthorizedError), UnauthorizedError.NoPermissionWhen)]
    public Response SetUserPermissions(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiSetUserPermissionsRequest body)
    {
        GameUser? userToSetPermissionsOf = database.GetUserWithId(id);
        if (userToSetPermissionsOf == null) return HttpStatusCode.NotFound;
        
        if (body.PermissionsType > user.PermissionsType || userToSetPermissionsOf.PermissionsType >= user.PermissionsType) 
            return new Response(UnauthorizedError.NoPermissionWhen, ContentType.Plaintext, HttpStatusCode.Unauthorized);
        
        database.SetUserPermissions(userToSetPermissionsOf, body.PermissionsType);
        return HttpStatusCode.Created;
    }
}
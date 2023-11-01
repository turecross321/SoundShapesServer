using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoDeletionPermissionWhen)]
    [DocRouteParam("id", "User ID.")]
    public ApiOkResponse RemoveUser(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameUser? userToRemove = database.GetUserWithId(id);
        if (userToRemove == null) 
            return ApiNotFoundError.UserNotFound;

        if (userToRemove.PermissionsType >= user.PermissionsType) 
            return ApiUnauthorizedError.NoDeletionPermission;
        
        database.RemoveUser(userToRemove, dataStore);
        return new ApiOkResponse();
    }

    [ApiEndpoint("users/id/{id}/setPermissions", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the permissions of user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoPermissionWhen)]
    [DocRouteParam("id", "User ID.")]
    public ApiOkResponse SetUserPermissions(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiSetUserPermissionsRequest body)
    {
        GameUser? userToSetPermissionsOf = database.GetUserWithId(id);
        if (userToSetPermissionsOf == null)
            return ApiNotFoundError.UserNotFound;
        
        if (body.PermissionsType > user.PermissionsType || userToSetPermissionsOf.PermissionsType >= user.PermissionsType) 
            return ApiUnauthorizedError.NoPermission;
        
        database.SetUserPermissions(userToSetPermissionsOf, body.PermissionsType);
        return new ApiOkResponse();
    }
}
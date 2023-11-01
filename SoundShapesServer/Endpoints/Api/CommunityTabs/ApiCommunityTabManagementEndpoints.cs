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
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.CommunityTabs;

public class ApiCommunityTabManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("communityTabs/create", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocError(typeof(ApiConflictError), ApiConflictError.TooManyCommunityTabsWhen)]
    public ApiResponse<ApiCommunityTabResponse> CreateCommunityTab(RequestContext context, GameDatabaseContext database,
        IDataStore dataStore,
        GameUser user, ApiCreateCommunityTabRequest body)
    {
        CommunityTab? createdCommunityTab = database.CreateCommunityTab(body, user);
        if (createdCommunityTab == null)
            return ApiConflictError.TooManyCommunityTabs;

        return ApiCommunityTabResponse.FromOld(createdCommunityTab);
    }

    [ApiEndpoint("communityTabs/id/{id}/edit", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits community tab with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.CommunityTabNotFoundWhen)]
    [DocRouteParam("id", "Community tab ID.")]
    public ApiResponse<ApiCommunityTabResponse> EditCommunityTab(RequestContext context, GameDatabaseContext database,
        IDataStore dataStore,
        GameUser user, ApiCreateCommunityTabRequest body, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null)
            return ApiNotFoundError.CommunityTabNotFound;

        CommunityTab editedCommunityTab = database.EditCommunityTab(communityTab, body, user);
        return ApiCommunityTabResponse.FromOld(editedCommunityTab);
    }

    [ApiEndpoint("communityTabs/id/{id}/setThumbnail", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of community tab with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.CommunityTabNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    [DocRouteParam("id", "Community tab ID.")]
    public ApiOkResponse SetCommunityTabThumbnail(RequestContext context, GameDatabaseContext database,
        IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null)
            return ApiNotFoundError.CommunityTabNotFound;

        return database.UploadCommunityTabResource(dataStore, communityTab, body);
    }

    [ApiEndpoint("communityTabs/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes community tab with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.CommunityTabNotFoundWhen)]
    [DocRouteParam("id", "Community tab ID.")]
    public ApiOkResponse RemoveCommunityTab(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null)
            return ApiNotFoundError.CommunityTabNotFound;

        database.RemoveCommunityTab(dataStore, communityTab);
        return new ApiOkResponse();
    }
}
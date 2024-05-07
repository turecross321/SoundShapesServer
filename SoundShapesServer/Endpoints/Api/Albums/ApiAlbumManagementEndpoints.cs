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
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/create", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates an album.")]
    public ApiResponse<ApiAlbumResponse> CreateAlbum(RequestContext context, GameDatabaseContext database,
        GameUser user, ApiAlbumRequest body)
    {
        GameAlbum album = database.CreateAlbum(body, user);
        return ApiAlbumResponse.FromOld(album);
    }

    [ApiEndpoint("albums/id/{id}/edit", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocRouteParam("id", "Album ID.")]
    public ApiResponse<ApiAlbumResponse> EditAlbum(RequestContext context, GameDatabaseContext database, GameUser user,
        ApiAlbumRequest body, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;

        GameAlbum editedAlbum = database.EditAlbum(album, body, user);
        return ApiAlbumResponse.FromOld(editedAlbum);
    }

    [ApiEndpoint("albums/id/{id}/setThumbnail", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of album.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    [DocRouteParam("id", "Album ID.")]
    public ApiOkResponse SetAlbumThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, byte[] body, string id)
    {
        return SetAlbumAssets(
            database,
            dataStore,
            body,
            id,
            AlbumResourceType.Thumbnail
        );
    }

    [ApiEndpoint("albums/id/{id}/setSidePanel", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets side panel of album.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    [DocRouteParam("id", "Album ID.")]
    public ApiOkResponse SetAlbumSidePanel(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, byte[] body, string id)
    {
        return SetAlbumAssets(
            database,
            dataStore,
            body,
            id,
            AlbumResourceType.SidePanel
        );
    }

    private ApiOkResponse SetAlbumAssets(GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id, AlbumResourceType resourceType)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;

        return database.UploadAlbumResource(dataStore, album, body, resourceType);
    }

    [ApiEndpoint("albums/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocRouteParam("id", "Album ID.")]
    public ApiOkResponse RemoveAlbum(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, string id)
    {
        GameAlbum? albumToRemove = database.GetAlbumWithId(id);
        if (albumToRemove == null)
            return ApiNotFoundError.AlbumNotFound;

        database.RemoveAlbum(dataStore, albumToRemove);
        return new ApiOkResponse();
    }
}
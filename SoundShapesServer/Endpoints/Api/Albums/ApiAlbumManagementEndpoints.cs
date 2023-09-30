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
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates an album.")]
    public ApiResponse<ApiAlbumResponse> CreateAlbum(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateAlbumRequest body)
    {
        GameAlbum album = database.CreateAlbum(body);
        return new ApiAlbumResponse(album);
    }

    [ApiEndpoint("albums/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    public ApiResponse<ApiAlbumResponse> EditAlbum(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateAlbumRequest body, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) 
            return ApiNotFoundError.AlbumNotFound;
        
        GameAlbum editedAlbum = database.EditAlbum(album, body);
        return new ApiAlbumResponse(editedAlbum);
    }

    [ApiEndpoint("albums/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of album.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    public ApiOkResponse SetAlbumThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, byte[] body, string id) 
        => SetAlbumAssets(
            database,
            dataStore,
            user,
            body, 
            id,
            AlbumResourceType.Thumbnail
    );
    
    [ApiEndpoint("albums/id/{id}/setSidePanel", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets side panel of album.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    public ApiOkResponse SetAlbumSidePanel(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, byte[] body, string id) 
        => SetAlbumAssets(
            database,
            dataStore,
            user,
            body, 
            id,
            AlbumResourceType.SidePanel
        );
    
    private ApiOkResponse SetAlbumAssets(GameDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body, string id, AlbumResourceType resourceType)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) 
            return ApiNotFoundError.AlbumNotFound;

        return database.UploadAlbumResource(dataStore, album, body, resourceType);
    }

    [ApiEndpoint("albums/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    public ApiOkResponse RemoveAlbum(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameAlbum? albumToRemove = database.GetAlbumWithId(id);
        if (albumToRemove == null)
            return ApiNotFoundError.AlbumNotFound;
        
        database.RemoveAlbum(dataStore, albumToRemove);
        return new ApiOkResponse();
    }
}
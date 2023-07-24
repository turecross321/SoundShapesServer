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
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates an album.")]
    public Response CreateAlbum(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateAlbumRequest body)
    {
        GameAlbum album = database.CreateAlbum(body);
        return new Response(new ApiAlbumResponse(album), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("albums/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits album with specified ID.")]
    public Response EditAlbum(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateAlbumRequest body, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;
        
        GameAlbum editedAlbum = database.EditAlbum(album, body);
        return new Response(new ApiAlbumResponse(editedAlbum), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("albums/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of album.")]
    public Response SetAlbumThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
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
    public Response SetAlbumSidePanel(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, byte[] body, string id) 
        => SetAlbumAssets(
            database,
            dataStore,
            user,
            body, 
            id,
            AlbumResourceType.SidePanel
        );
    
    private Response SetAlbumAssets(GameDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body, string id, AlbumResourceType resourceType)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;

        return database.UploadAlbumResource(dataStore, album, body, resourceType);
    }

    [ApiEndpoint("albums/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes album with specified ID.")]
    public Response RemoveAlbum(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameAlbum? albumToRemove = database.GetAlbumWithId(id);
        if (albumToRemove == null) return HttpStatusCode.NotFound;
        
        database.RemoveAlbum(dataStore, albumToRemove);
        return HttpStatusCode.OK;
    }
}
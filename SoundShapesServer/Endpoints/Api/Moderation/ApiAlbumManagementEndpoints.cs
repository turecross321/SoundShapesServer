using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiAlbumManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/create", Method.Post)]
    public Response CreateAlbum(RequestContext context, RealmDatabaseContext database, GameUser user, ApiCreateAlbumRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameAlbum album = database.CreateAlbum(body);
        return new Response(new ApiAlbumResponse(album), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("albums/{id}/edit", Method.Post)]
    public Response EditAlbum(RequestContext context, RealmDatabaseContext database, GameUser user, ApiCreateAlbumRequest body, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;
        
        GameAlbum editedAlbum = database.EditAlbum(album, body);
        return new Response(new ApiAlbumResponse(editedAlbum), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("albums/{id}/setThumbnail", Method.Post)]
    public Response SetAlbumThumbnail(RequestContext context, RealmDatabaseContext database, IDataStore dataStore,
        GameUser user, Stream body, string id) 
        => SetAlbumAssets(
            database,
            dataStore,
            user,
            body, 
            id,
            AlbumResourceType.Thumbnail
    );
    
    [ApiEndpoint("albums/{id}/setSidePanel", Method.Post)]
    public Response SetAlbumSidePanel(RequestContext context, RealmDatabaseContext database, IDataStore dataStore,
        GameUser user, Stream body, string id) 
        => SetAlbumAssets(
            database,
            dataStore,
            user,
            body, 
            id,
            AlbumResourceType.SidePanel
        );
    
    private Response SetAlbumAssets(RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body, string id, AlbumResourceType resourceType)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;

        byte[] image;

        using (MemoryStream memoryStream = new ())
        {
            body.CopyTo(memoryStream);
            image = memoryStream.ToArray();
        }
        
        if (!IsByteArrayPng(image)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        dataStore.WriteToStore(GetAlbumResourceKey(album.Id, resourceType), image);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("albums/{id}/remove", Method.Post)]
    public Response RemoveAlbum(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameAlbum? albumToRemove = database.GetAlbumWithId(id);
        if (albumToRemove == null) return HttpStatusCode.NotFound;
        
        database.RemoveAlbum(albumToRemove);
        return HttpStatusCode.OK;
    }
}
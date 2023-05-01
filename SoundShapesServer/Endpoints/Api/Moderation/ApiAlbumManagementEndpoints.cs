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
using SoundShapesServer.Types.Albums;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiAlbumManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/create", Method.Post)]
    public Response CreateAlbum(RequestContext context, RealmDatabaseContext database, GameUser user, ApiCreateAlbumRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        database.CreateAlbum(body);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("album/{id}/edit", Method.Post)]
    public Response EditAlbum(RequestContext context, RealmDatabaseContext database, GameUser user, ApiCreateAlbumRequest body, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;
        
        database.EditAlbum(album, body);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("album/{id}/set{assetType}", Method.Post)]
    public Response SetAlbumAssets(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body, string id, string resource)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;
        
        if (!Enum.TryParse(resource, out AlbumResourceType resourceType)) return HttpStatusCode.NotFound;
        
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
}
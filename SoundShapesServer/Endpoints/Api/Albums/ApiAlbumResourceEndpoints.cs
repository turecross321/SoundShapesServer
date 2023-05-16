using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumResourceEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response GetAlbumThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database,
        string id)
        => GetAlbumResource(dataStore, database, id, AlbumResourceType.Thumbnail);
    
    [ApiEndpoint("albums/id/{id}/sidePanel")]
    [Authentication(false)]
    public Response GetAlbumSidePanel(RequestContext context, IDataStore dataStore, GameDatabaseContext database,
        string id)
        => GetAlbumResource(dataStore, database, id, AlbumResourceType.SidePanel);
    
    private Response GetAlbumResource(IDataStore dataStore, GameDatabaseContext database, string id, AlbumResourceType resourceType)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return HttpStatusCode.NotFound;

        string key = ResourceHelper.GetAlbumResourceKey(id, resourceType);

        if (!dataStore.ExistsInStore(key))
            return HttpStatusCode.NotFound;

        if (!dataStore.TryGetDataFromStore(key, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }
}
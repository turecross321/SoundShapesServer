using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
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

        string? key = resourceType switch
        {
            AlbumResourceType.Thumbnail => album.ThumbnailFilePath,
            AlbumResourceType.SidePanel => album.SidePanelFilePath,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
        };

        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
}
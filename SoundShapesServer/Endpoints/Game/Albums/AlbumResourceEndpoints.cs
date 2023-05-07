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

namespace SoundShapesServer.Endpoints.Game.Albums;

public class AlbumResourceEndpoints : EndpointGroup
{
    private static Response GetResource(IDataStore dataStore, string fileName)
    {
        if (!dataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.NotFound;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [GameEndpoint("~album:{albumId}/~content:{resource}/data.get")]
    [Authentication(false)]
    public Response GetAlbumResource
        (RequestContext context, IDataStore dataStore, GameDatabaseContext database, string albumId, string resource)
    {
        if (database.GetAlbumWithId(albumId) == null) return HttpStatusCode.NotFound;

        if (!Enum.TryParse(resource, out AlbumResourceType resourceType)) return HttpStatusCode.NotFound;
        string key = ResourceHelper.GetAlbumResourceKey(albumId, resourceType);

        return GetResource(dataStore, key);
    }
}
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

    [GameEndpoint("~album:{albumId}/~content:{resource}/data.get/{sessionId}")]
    [Authentication(false)] // Vita doesn't include the session id in the headers here, hence the session id being in the url
    public Response GetAlbumResource
        (RequestContext context, IDataStore dataStore, RealmDatabaseContext database, string albumId, string resource, string sessionId)
    {
        if (database.IsSessionInvalid(sessionId)) return HttpStatusCode.Forbidden;
        if (database.GetAlbumWithId(albumId) == null) return HttpStatusCode.NotFound;

        if (!Enum.TryParse(resource, out AlbumResourceType resourceType)) return HttpStatusCode.NotFound;
        string key = ResourceHelper.GetAlbumResourceKey(albumId, resourceType);

        return GetResource(dataStore, key);
    }
}
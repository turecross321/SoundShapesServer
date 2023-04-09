using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Endpoints.Game.Albums;

public class AlbumResourceEndpoints : EndpointGroup
{
    private static Response GetResource(RequestContext context, string fileName)
    {
        if (!context.DataStore.ExistsInStore(fileName))
            return HttpStatusCode.NotFound;

        if (!context.DataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [GameEndpoint("~album:{albumId}/~content:{file}/data.get/{sessionId}")]
    [Authentication(false)] // Vita doesn't include the session id in the headers here, hence the session id being in the url
    public Response GetAlbumResource
        (RequestContext context, RealmDatabaseContext database, string albumId, string file, string sessionId)
    {
        if (database.IsSessionInvalid(sessionId)) return HttpStatusCode.Forbidden;
        
        if (database.GetAlbumWithId(albumId) == null) return HttpStatusCode.NotFound;

        string key = ResourceHelper.GetAlbumResourceKey(albumId, file);

        return GetResource(context, key);
    }
}
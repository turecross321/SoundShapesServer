using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsResourceEndpoints : EndpointGroup
{
    [ApiEndpoint("news/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response NewsThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        string? key = newsEntry?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
}
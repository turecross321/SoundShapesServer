using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Endpoints.Game;

public class NewsResourceEndpoints : EndpointGroup
{
    [GameEndpoint("~news:{id}/~content:thumbnail/data.get", ContentType.BinaryData)]
    [Authentication(false)]
    public Response GetNewsImage(RequestContext context, GameDatabaseContext database, IDataStore dataStore, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        string? key = newsEntry.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
}
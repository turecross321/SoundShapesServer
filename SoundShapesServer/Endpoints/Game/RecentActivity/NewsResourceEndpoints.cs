using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class NewsResourceEndpoints : EndpointGroup
{
    [GameEndpoint("~news:{id}/~content:thumbnail/data.get", ContentType.BinaryData)]
    [Authentication(false)]
    public Response GetNewsImage(RequestContext context, IDataStore dataStore, string id)
    {
        dataStore.TryGetDataFromStore(ResourceHelper.GetNewsResourceKey(id), out byte[]? data);
        if (data == null) return HttpStatusCode.NotFound;
        
        return new Response(data, ContentType.BinaryData);
    }
}
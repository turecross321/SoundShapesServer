using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelResourceEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response GetLevelThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        string? key = level?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [ApiEndpoint("levels/id/{id}/level")]
    [Authentication(false)]
    public Response GetLevelFile(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        string? key = level?.LevelFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
}
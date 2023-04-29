using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api;

public class ApiLevelResourceEndpoints : EndpointGroup
{
    [ApiEndpoint("level/{levelId}/thumbnail")]
    [Authentication(false)]
    public Response LevelThumbnail(RequestContext context, IDataStore dataStore, RealmDatabaseContext database, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        string key = ResourceHelper.GetLevelResourceKey(level.Id, FileType.Image);

        if (!dataStore.ExistsInStore(key))
            return HttpStatusCode.NotFound;

        if (!dataStore.TryGetDataFromStore(key, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }
}
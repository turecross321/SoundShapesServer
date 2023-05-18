using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelResourceEndpoints : EndpointGroup
{
    private static Response GetResource(IDataStore dataStore, string fileName)
    {
        if (!dataStore.ExistsInStore(fileName))
            return HttpStatusCode.NotFound;

        if (!dataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [GameEndpoint("~level:{levelId}/~version:{versionId}/~content:{file}/data.get")]
    public Response GetLevelResource
        (RequestContext context, IDataStore dataStore, GameDatabaseContext database, GameUser user, string levelId, string versionId, string file)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        FileType fileType = GetFileTypeFromName(file);

        string? key = fileType switch
        {
            FileType.Level => level.LevelFilePath,
            FileType.Image => level.ThumbnailFilePath,
            FileType.Sound => level.SoundFilePath,
            FileType.Unknown => null,
            _ => null
        };

        return key == null ? HttpStatusCode.NotFound : GetResource(dataStore, key);
    }
}
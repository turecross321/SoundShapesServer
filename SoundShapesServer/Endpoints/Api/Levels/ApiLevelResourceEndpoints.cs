using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelResourceEndpoints : EndpointGroup
{
    // Called from API Publishing Endpoints
    public static bool UploadLevelResources(IDataStore dataStore, MultipartFormDataParser parser, string levelId)
    {
        if (parser.GetParameterValue("Title").Length > 26) return false;

        FilePart? imagePart = parser.Files.FirstOrDefault(f => f.Name == "Image");
        FilePart? levelPart = parser.Files.FirstOrDefault(f => f.Name == "Level");
        FilePart? soundPart = parser.Files.FirstOrDefault(f => f.Name == "Sound");

        if (imagePart == null || levelPart == null || soundPart == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return false;
        }

        string imageKey = GetLevelResourceKey(levelId, FileType.Image);
        string levelKey = GetLevelResourceKey(levelId, FileType.Level);
        string soundKey = GetLevelResourceKey(levelId, FileType.Sound);
        
        byte[] image = FilePartToBytes(imagePart);
        byte[] level = FilePartToBytes(levelPart);
        byte[] sound = FilePartToBytes(soundPart);

        dataStore.WriteToStore(imageKey, image);
        dataStore.WriteToStore(levelKey, level);
        dataStore.WriteToStore(soundKey, sound);

        return true;
        // todo: move these kinds of things to the publish endpoint tbh
    }
    
    [ApiEndpoint("level/{levelId}/thumbnail")]
    [Authentication(false)]
    public Response LevelThumbnail(RequestContext context, IDataStore dataStore, RealmDatabaseContext database, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        string key = GetLevelResourceKey(level.Id, FileType.Image);

        if (!dataStore.ExistsInStore(key))
            return HttpStatusCode.NotFound;

        if (!dataStore.TryGetDataFromStore(key, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }
}
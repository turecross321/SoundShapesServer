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

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelResourceEndpoints : EndpointGroup
{
    // Called from Publishing Endpoints
    public static bool UploadLevelResources(IDataStore dataStore, MultipartFormDataParser parser, string levelId)
    {
        if (parser.GetParameterValue("title").Length > 26) return false;

        byte[]? image = null;
        byte[]? level = null;
        byte[]? sound = null;
        
        foreach (FilePart? file in parser.Files)
        {
            byte[] bytes = FilePartToBytes(file);
            FileType fileType = GetFileTypeFromFilePart(file);

            switch (fileType)
            {
                case FileType.Image:
                    image = bytes;
                    break;
                case FileType.Level:
                    level = bytes;
                    break;
                case FileType.Sound:
                    sound = bytes;
                    break;
                default:
                    Console.WriteLine("User attempted to upload an unaccounted-for file: " + file.ContentType);
                    return true;
            }
        }

        if (image == null || level == null || sound == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return false;
        }

        string imageKey = GetLevelResourceKey(levelId, FileType.Image);
        string levelKey = GetLevelResourceKey(levelId, FileType.Level);
        string soundKey = GetLevelResourceKey(levelId, FileType.Sound);

        dataStore.WriteToStore(imageKey, image);
        dataStore.WriteToStore(levelKey, level);
        dataStore.WriteToStore(soundKey, sound);

        return true;
    }
    // Called from Publishing Endpoints
    public static void RemoveLevelResources(IDataStore dataStore, GameLevel level)
    {
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Image));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Level));
        dataStore.RemoveFromStore(GetLevelResourceKey(level.Id, FileType.Sound));
    }
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
        (RequestContext context, IDataStore dataStore, RealmDatabaseContext database, GameUser user, string levelId, string versionId, string file)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        FileType fileType = GetFileTypeFromName(file);
        
        string key = GetLevelResourceKey(level.Id, fileType);

        return GetResource(dataStore, key);
    }
}
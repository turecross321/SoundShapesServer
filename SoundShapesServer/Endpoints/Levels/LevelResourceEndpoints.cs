using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelResourceEndpoints : EndpointGroup
{
    // Called from Publishing Endpoints
    public static LevelPublishRequest? UploadLevelResources(RequestContext context, MultipartFormDataParser parser, string levelId)
    {
        FilePart? image = null;
        FilePart? level = null;
        FilePart? sound = null;
        
        foreach (FilePart? file in parser.Files)
        {
            switch (file.ContentType)
            {
                case IFileType.Image:
                    image = file;
                    break;
                case IFileType.Level:
                    level = file;
                    break;
                case IFileType.Sound:
                    sound = file;
                    break;
                default:
                    Console.WriteLine("User attempted to upload an unaccounted-for file: " + file.ContentType);
                    return null;
            }
        }

        if (image == null || level == null || sound == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return null;
        }

        foreach (FilePart? file in parser.Files)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.Data.CopyTo(memoryStream);
            byte[] byteArray = memoryStream.ToArray();
            
            string key = ResourceHelper.GetLevelResourceKey(levelId, file.ContentType);

            context.DataStore.WriteToStore(key, byteArray);
        }

        LevelPublishRequest levelRequest = new ()
        {
            Title = parser.GetParameterValue("title"),
            Description = parser.GetParameterValue("description"),
            Icon = image,
            Level = level,
            Song = sound,
            Language = int.Parse(parser.GetParameterValue("sce_np_language")),
            Id = levelId
        };
        
        return levelRequest;
    }
    // Called from Publishing Endpoints
    public static void RemoveLevelResources(RequestContext context, GameLevel level)
    {
        context.DataStore.RemoveFromStore(ResourceHelper.GetLevelResourceKey(level.Id, IFileType.Image));
        context.DataStore.RemoveFromStore(ResourceHelper.GetLevelResourceKey(level.Id, IFileType.Level));
        context.DataStore.RemoveFromStore(ResourceHelper.GetLevelResourceKey(level.Id, IFileType.Sound));
    }
    private static Response GetResource(RequestContext context, string fileName)
    {
        if (!context.DataStore.ExistsInStore(fileName))
            return HttpStatusCode.NotFound;

        if (!context.DataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [Endpoint("/otg/~level:{levelId}/~version:{versionId}/~content:{file}/data.get")]
    public Response GetLevelResource
        (RequestContext context, RealmDatabaseContext database, GameUser user, string levelId, string versionId, string file)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        string key = ResourceHelper.GetLevelResourceKey(level.Id, file);

        return GetResource(context, key);
    }
}
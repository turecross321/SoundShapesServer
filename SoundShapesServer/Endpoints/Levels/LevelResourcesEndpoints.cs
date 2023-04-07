using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using Newtonsoft.Json.Serialization;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelResourcesEndpoints : EndpointGroup
{
    // Called from Publishing Endpoints
    public static LevelPublishRequest UploadResources(RequestContext context, MultipartFormDataParser parser, string levelId)
    {
        FilePart? image = null;
        FilePart? level = null;
        FilePart? sound = null;
        
        foreach (FilePart? file in parser.Files)
        {
            switch (file.ContentType)
            {
                case "image/png":
                    image = file;
                    break;
                case "application/vnd.soundshapes.level":
                    level = file;
                    break;
                case "application/vnd.soundshapes.sound":
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
            
            string fileExtension = file.ContentType.Split("/")[1];
            
            context.DataStore.WriteToStore($"{levelId}.{fileExtension}", byteArray);
        }

        LevelPublishRequest levelRequest = new LevelPublishRequest()
        {
            title = parser.GetParameterValue("title"),
            description = parser.GetParameterValue("description"),
            image = image,
            level = level,
            sound = sound,
            sce_np_language = int.Parse(parser.GetParameterValue("sce_np_language")),
            levelId = levelId
        };
        
        return levelRequest;
    }
    private Response GetResource(RequestContext context, string levelId, FileType fileType)
    {
        string fileName = levelId;

        string fileExtension = "";
        
        switch (fileType)
        {
            case FileType.image:
                fileExtension = ".png";
                break;
            case FileType.level:
                fileExtension = ".vnd.soundshapes.level";
                break;
            case FileType.sound:
                fileExtension = ".vnd.soundshapes.sound";
                break;
            default:
                return HttpStatusCode.NotFound;
        }

        fileName += fileExtension;
        
        if (!context.DataStore.ExistsInStore(fileName))
            return HttpStatusCode.NotFound;

        if (!context.DataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [Endpoint("/otg/~level:{levelId}/~version:{versionId}/~content:{fileTypeString}/data.get")]
    public Response GetLevelResource(RequestContext context, RealmDatabaseContext database, GameUser user, string levelId, string versionId, string fileTypeString)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;
        
        Enum.TryParse(fileTypeString, true, out FileType fileType);

        return GetResource(context, levelId, fileType);
    }
}
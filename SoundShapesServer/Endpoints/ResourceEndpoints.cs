using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class ResourceEndpoints : EndpointGroup
{
    // Called from Publishing Endpoints
    public static LevelPublishRequest? UploadResources(RequestContext context, MultipartFormDataParser parser, string levelId)
    {
        FilePart? image = null;
        FilePart? level = null;
        FilePart? sound = null;
        
        foreach (FilePart? file in parser.Files)
        {
            switch (file.ContentType)
            {
                case IFileType.image:
                    image = file;
                    break;
                case IFileType.level:
                    level = file;
                    break;
                case IFileType.sound:
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

            string fileName = levelId;
            // gets the .level from application/vnd.soundshapes.level e.g.
            string fileExtension = file.ContentType.Split("/").Last().Split(".").Last();

            string key = $"{fileName}.{fileExtension}";
            
            context.DataStore.WriteToStore(key, byteArray);
        }

        LevelPublishRequest levelRequest = new ()
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

        string fileExtension = "";

        switch (file)
        {
            case "image":
                fileExtension = "png";
                break;
            case "level":
                fileExtension = "level";
                break;
            case "sound":
                fileExtension = "sound";
                break;
            default:
                return HttpStatusCode.NotFound;
        }
        
        string fileName = levelId;

        string key = fileName + "." + fileExtension;

        return GetResource(context, key);
    }

    [Endpoint("/otg/~album:{albumId}/~content:{file}/data.get")]
    public Response GetAlbumResource
        (RequestContext context, RealmDatabaseContext database, string albumId, string file)
    {
        if (database.GetAlbumWithId(albumId) == null) return HttpStatusCode.NotFound;
        
        string key = albumId + "_" + file;
        
        return GetResource(context, key);
    }
}
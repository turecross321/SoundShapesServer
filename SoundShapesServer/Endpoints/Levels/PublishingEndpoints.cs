using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class PublishingEndpoints : EndpointGroup
{
    [Endpoint("/otg/~level:*.create", Method.Post, ContentType.Json)]
    public static Response Publish(RequestContext context, Stream body, RealmDatabaseContext database, GameUser user)
    {
        var parser = MultipartFormDataParser.Parse(body);

        FilePart? image = null;
        FilePart? level = null;
        FilePart? sound = null;
        
        foreach (var file in parser.Files)
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
                    return new Response(HttpStatusCode.BadRequest);
                    break;
            }
        }

        if (image == null || level == null || sound == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return new Response(HttpStatusCode.BadRequest);
        }

        string levelId = database.GenerateLevelId();

        foreach (var file in parser.Files)
        {
            MemoryStream memoryStream = new MemoryStream();
            file.Data.CopyTo(memoryStream);
            byte[] byteArray = memoryStream.ToArray();
            
            string fileExtension = file.ContentType.Split("/")[1];
            
            context.DataStore.WriteToStore($"{levelId}.{fileExtension}", byteArray);
        }
        
        var levelRequest = new LevelPublishRequest()
        {
            title = parser.GetParameterValue("title"),
            description = parser.GetParameterValue("description"),
            image = image,
            level = level,
            sound = sound,
            sce_np_language = int.Parse(parser.GetParameterValue("sce_np_language")),
            levelId = levelId
        };

        LevelPublishResponse publishedLevel = database.PublishLevel(levelRequest, user);

        return new Response(publishedLevel, ContentType.Json, HttpStatusCode.Created);
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiLevelPublishingEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/create", Method.Post)]
    public Response PublishLevels(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        MultipartFormDataParser parser = MultipartFormDataParser.Parse(body);

        string levelId = LevelHelper.GenerateLevelId(database);
        Response? uploadedResources = UploadLevelResources(dataStore, parser, levelId);
        if (uploadedResources == null) return uploadedResources ?? HttpStatusCode.InternalServerError;

        bool suppliedModified = DateTimeOffset.TryParse(parser.GetParameterValue("Modified"), out DateTimeOffset modified);

        LevelPublishRequest levelRequest = new(
            parser.GetParameterValue("Name"),
            int.Parse(parser.GetParameterValue("Language")),
            levelId,
            parser.Files.First(f => f.Name == "level").Data.Length,
            suppliedModified ? modified : null
            );

        database.PublishLevel(levelRequest, user);

        return new Response(HttpStatusCode.Created);
    }

    private Response? UploadLevelResources(IDataStore dataStore, IMultipartFormDataParser parser, string levelId)
    {
        if (parser.GetParameterValue("Name").Length > 26) 
            return new Response("Illegal level name.", ContentType.Plaintext, HttpStatusCode.BadRequest);

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
                case FileType.Unknown:
                default:
                    return new Response("Illegal file type.", ContentType.Plaintext, HttpStatusCode.BadRequest);
            }
        }

        if (image == null) 
            return new Response("No thumbnail provided.", ContentType.Plaintext, HttpStatusCode.BadRequest); 
        if (level == null)
            return new Response("No level file provided.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        if (sound == null)
            return new Response("No sound file provided.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        string imageKey = GetLevelResourceKey(levelId, FileType.Image);
        string levelKey = GetLevelResourceKey(levelId, FileType.Level);
        string soundKey = GetLevelResourceKey(levelId, FileType.Sound);

        dataStore.WriteToStore(imageKey, image);
        dataStore.WriteToStore(levelKey, level);
        dataStore.WriteToStore(soundKey, sound);

        return null;
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Game.Levels;

// ReSharper disable once ClassNeverInstantiated.Global
public class LevelPublishingEndpoints : EndpointGroup
{
    // Gets called by Endpoints.cs
    public static Response CreateLevel(GameServerConfig config, IDataStore dataStore, MultipartFormDataParser parser, GameDatabaseContext database, GameUser user)
    {
        if (user.Levels.Count() >= config.LevelPublishLimit) return HttpStatusCode.Forbidden;

        PublishLevelRequest publishLevelRequest = new (
            parser.GetParameterValue("title"), 
            int.Parse(parser.GetParameterValue("sce_np_language")),
            parser.Files.First(f => f.Name == "level").Data.Length);

        GameLevel publishedLevel = database.CreateLevel(publishLevelRequest, user);
        
        Response? uploadedResources = UploadLevelResources(dataStore, parser, publishedLevel.Id);
        if (uploadedResources != null) return (Response)uploadedResources;
        
        return new Response(new LevelPublishResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UpdateLevel(IDataStore dataStore, MultipartFormDataParser parser, GameDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return new Response(HttpStatusCode.NotFound);
        if (level.Author?.Id != user.Id) return new Response(HttpStatusCode.Forbidden);
        
        if (user.Id  != level.Author.Id) return HttpStatusCode.Unauthorized;

        PublishLevelRequest publishLevelRequest = new (
            parser.GetParameterValue("title"), 
            int.Parse(parser.GetParameterValue("sce_np_language")),
            parser.Files.First(f => f.Name == "level").Data.Length);
        
        GameLevel publishedLevel = database.EditLevel(publishLevelRequest, level);
        
        Response? uploadedResources = UploadLevelResources(dataStore, parser, levelId);
        if (uploadedResources != null) return (Response)uploadedResources;
        
        return new Response(new LevelPublishResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }


    private static Response? UploadLevelResources(IDataStore dataStore, IMultipartFormDataParser parser, string levelId)
    {
        if (parser.GetParameterValue("title").Length > 26) return HttpStatusCode.BadRequest;

        byte[]? level = null;
        byte[]? image = null;
        byte[]? sound = null;
        
        foreach (FilePart? file in parser.Files)
        {
            byte[] bytes = FilePartToBytes(file);
            FileType fileType = GetFileTypeFromFilePart(file);

            switch (fileType)
            {
                case FileType.Level:
                    level = bytes;
                    break;
                case FileType.Image:
                    image = bytes;
                    break;
                case FileType.Sound:
                    sound = bytes;
                    break;
                case FileType.Unknown:
                default:
                    Console.WriteLine("User attempted to upload an illegal file: " + file.ContentType);
                    return HttpStatusCode.BadRequest;
            }
        }

        if (level == null || image == null || sound == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return HttpStatusCode.BadRequest;
        }

        string levelKey = GetLevelResourceKey(levelId, FileType.Level);
        string imageKey = GetLevelResourceKey(levelId, FileType.Image);
        string soundKey = GetLevelResourceKey(levelId, FileType.Sound);

        dataStore.WriteToStore(levelKey, level);
        dataStore.WriteToStore(imageKey, image);
        dataStore.WriteToStore(soundKey, sound);

        return null;
    }
    
    // Gets called by Endpoints.cs
    public static Response UnPublishLevel(IDataStore dataStore, GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (level.Author?.Id != user.Id) return new Response(HttpStatusCode.Forbidden);
        
        database.RemoveLevel(level, dataStore);

        return HttpStatusCode.OK;
    }
}
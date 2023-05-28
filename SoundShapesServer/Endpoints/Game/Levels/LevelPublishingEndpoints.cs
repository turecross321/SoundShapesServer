using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Bunkum.ProfanityFilter;
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
    public static Response CreateLevel(GameServerConfig config, IDataStore dataStore, ProfanityService profanity, MultipartFormDataParser parser, GameDatabaseContext database, GameUser user)
    {
        if (user.Levels.Count() >= config.LevelPublishLimit) return HttpStatusCode.Forbidden;

        PublishLevelRequest publishLevelRequest = new (
            parser.GetParameterValue("title"), 
            int.Parse(parser.GetParameterValue("sce_np_language")));
        
        publishLevelRequest.Name = profanity.CensorSentence(publishLevelRequest.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.CreateLevel(publishLevelRequest, user);
        
        Response uploadedResources = UploadLevelResources(database, dataStore, parser, publishedLevel);
        if (uploadedResources.StatusCode != HttpStatusCode.Created) return uploadedResources;
        
        return new Response(new LevelPublishResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UpdateLevel(IDataStore dataStore, ProfanityService profanity, MultipartFormDataParser parser, GameDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return HttpStatusCode.NotFound;
        if (level.Author.Id != user.Id) return HttpStatusCode.Forbidden;

        PublishLevelRequest publishLevelRequest = new (
            parser.GetParameterValue("title"), 
            int.Parse(parser.GetParameterValue("sce_np_language")));
        
        publishLevelRequest.Name = profanity.CensorSentence(publishLevelRequest.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.EditLevel(publishLevelRequest, level);
        
        Response uploadedResources = UploadLevelResources(database, dataStore, parser, publishedLevel);
        if (uploadedResources.StatusCode != HttpStatusCode.Created) return uploadedResources;
        
        return new Response(new LevelPublishResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }


    private static Response UploadLevelResources(GameDatabaseContext database, IDataStore dataStore, 
        IMultipartFormDataParser parser, GameLevel level)
    {
        byte[]? levelFile = null;
        byte[]? thumbnailFile = null;
        byte[]? soundFile = null;
        
        foreach (FilePart? file in parser.Files)
        {
            byte[] bytes = FilePartToBytes(file);
            FileType fileType = GetFileTypeFromFilePart(file);

            switch (fileType)
            {
                case FileType.Level:
                    levelFile = bytes;
                    break;
                case FileType.Image:
                    thumbnailFile = bytes;
                    break;
                case FileType.Sound:
                    soundFile = bytes;
                    break;
                case FileType.Unknown:
                default:
                    Console.WriteLine("User attempted to upload an illegal file: " + file.ContentType);
                    return HttpStatusCode.BadRequest;
            }
        }

        if (levelFile == null || thumbnailFile == null || soundFile == null)
        {
            Console.WriteLine("User did not upload all the required files.");
            return HttpStatusCode.BadRequest;
        }

        database.UploadLevelResources(dataStore, level, levelFile, thumbnailFile, soundFile);

        return HttpStatusCode.Created;
    }
    
    // Gets called by Endpoints.cs
    public static Response UnPublishLevel(IDataStore dataStore, GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (level.Author.Id != user.Id) return new Response(HttpStatusCode.Forbidden);
        
        database.RemoveLevel(level, dataStore);

        return HttpStatusCode.OK;
    }
}
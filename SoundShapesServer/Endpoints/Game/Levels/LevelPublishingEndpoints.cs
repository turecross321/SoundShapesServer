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
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelPublishingEndpoints : EndpointGroup
{
    // Gets called by Endpoints.cs
    public static Response PublishLevel(IDataStore dataStore, MultipartFormDataParser parser, RealmDatabaseContext database, GameUser user)
    {
        string levelId = LevelHelper.GenerateLevelId(database);
        bool uploadedResources = LevelResourceEndpoints.UploadLevelResources(dataStore, parser, levelId);
        if (uploadedResources == false) return HttpStatusCode.BadRequest;
        
        LevelPublishRequest levelRequest = new ()
        {
            Title = parser.GetParameterValue("title"),
            Language = int.Parse(parser.GetParameterValue("sce_np_language")),
            Id = levelId,
            FileSize = parser.Files.First(f => f.Name == "level").Data.Length
        };

        LevelPublishResponse publishedLevel = database.PublishLevel(levelRequest, user);

        return new Response(publishedLevel, ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UpdateLevel(IDataStore dataStore, MultipartFormDataParser parser, RealmDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return new Response(HttpStatusCode.NotFound);
        if (level.Author.Id != user.Id) return new Response(HttpStatusCode.Forbidden);
        
        bool uploadedResources = LevelResourceEndpoints.UploadLevelResources(dataStore, parser, levelId);
        if (uploadedResources == false) return HttpStatusCode.BadRequest;

        LevelPublishRequest levelRequest = new ()
        {
            Title = parser.GetParameterValue("title"),
            Language = int.Parse(parser.GetParameterValue("sce_np_language")),
            Id = levelId,
            FileSize = parser.Files.First(f => f.Name == "level").Data.Length
        };
        
        LevelPublishResponse? publishedLevel = database.UpdateLevel(levelRequest, level, user);

        if (publishedLevel == null) return new Response(HttpStatusCode.InternalServerError);

        return new Response(publishedLevel, ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UnPublishLevel(IDataStore dataStore, RealmDatabaseContext database, GameUser user, GameLevel level)
    {
        if (level.Author.Id != user.Id) return new Response(HttpStatusCode.Forbidden);
        
        LevelResourceEndpoints.RemoveLevelResources(dataStore, level);
        return database.UnPublishLevel(level) ? new Response(HttpStatusCode.OK) : HttpStatusCode.InternalServerError;
    }
}
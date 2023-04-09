using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelPublishingEndpoints : EndpointGroup
{
    // Gets called by Endpoints.cs
    public static Response PublishLevel(RequestContext context, MultipartFormDataParser parser, RealmDatabaseContext database, GameUser user)
    {
        string levelId = LevelHelper.GenerateLevelId(database);
        
        LevelPublishRequest? levelRequest = LevelResourceEndpoints.UploadLevelResources(context, parser, levelId);
        if (levelRequest == null) return HttpStatusCode.InternalServerError;

        LevelPublishResponse publishedLevel = database.PublishLevel(levelRequest, user);

        return new Response(publishedLevel, ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UpdateLevel(RequestContext context, MultipartFormDataParser parser, RealmDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return new Response(HttpStatusCode.NotFound);
        if (!level.Author.Equals(user)) return new Response(HttpStatusCode.Forbidden);
        
        LevelPublishRequest? levelRequest = LevelResourceEndpoints.UploadLevelResources(context, parser, levelId);
        if (levelRequest == null) return HttpStatusCode.InternalServerError;

        
        LevelPublishResponse? publishedLevel = database.UpdateLevel(levelRequest, level, user);

        if (publishedLevel == null) return new Response(HttpStatusCode.InternalServerError);

        return new Response(publishedLevel, ContentType.Json, HttpStatusCode.Created);
    }
    
    // Gets called by Endpoints.cs
    public static Response UnPublishLevel(RequestContext context, RealmDatabaseContext database, GameUser user, GameLevel level)
    {
        if (level.Author.Equals(user) == false) return new Response(HttpStatusCode.Forbidden);  // Check if user is the level publisher
        
        LevelResourceEndpoints.RemoveLevelResources(context, level);
        return database.UnPublishLevel(level) ? new Response(HttpStatusCode.OK) : HttpStatusCode.InternalServerError;
    }
}
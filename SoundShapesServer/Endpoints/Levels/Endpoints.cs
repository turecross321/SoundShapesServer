using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

// Doing this because Bunkum doesn't support . as a seperator
public class Endpoints : EndpointGroup
{
    [Endpoint("/otg/~level:{args}")]
    public Response GetEndpoints(RequestContext context, RealmDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        GameLevel? level = database.GetLevelWithId(levelId);
        
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        if (action == "delete") return LevelPublishingEndpoints.UnPublishLevel(context, database, user, level);
        if (action == "latest") return new Response(LevelHelper.LevelToLevelResponse(level, user), ContentType.Json);

        else return new Response(HttpStatusCode.NotFound);
    }

    [Endpoint("/otg/~level:{args}", Method.Post)]
    public Response PostEndpoints(RequestContext context, Stream body, RealmDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        if (action == "create") return LevelPublishingEndpoints.PublishLevel(context, parser, database, user);
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        // RequestContext context, Stream body, RealmDatabaseContext database, GameUser user, string levelId
        
        if (action == "update") return LevelPublishingEndpoints.UpdateLevel(context, parser, database, user, level.id);

        else return new Response(HttpStatusCode.NotFound);
    }
}
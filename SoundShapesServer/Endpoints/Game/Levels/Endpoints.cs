using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game.Levels;

// Doing this because Bunkum doesn't support . as a seperator
public class Endpoints : EndpointGroup
{
    [GameEndpoint("~level:{args}")]
    public Response GetEndpoints(RequestContext context, IDataStore dataStore, RealmDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        GameLevel? level = database.GetLevelWithId(levelId);
        
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        if (action == "delete") return LevelPublishingEndpoints.UnPublishLevel(dataStore, database, user, level);
        if (action == "latest") return new Response(LevelHelper.LevelToLevelResponse(level, user), ContentType.Json);

        else return new Response(HttpStatusCode.NotFound);
    }

    [GameEndpoint("~level:{args}", Method.Post)]
    public Response PostEndpoints(RequestContext context, IDataStore dataStore, Stream body, RealmDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        if (action == "create") return LevelPublishingEndpoints.PublishLevel(dataStore, parser, database, user);
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);

        if (action == "update") return LevelPublishingEndpoints.UpdateLevel(dataStore, parser, database, user, level.Id);

        return new Response(HttpStatusCode.NotFound);
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Bunkum.ProfanityFilter;
using HttpMultipartParser;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class Endpoints : EndpointGroup
{
    [GameEndpoint("~level:{args}")]
    public Response GetEndpoints(RequestContext context, IDataStore dataStore, GameDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        GameLevel? level = database.GetLevelWithId(levelId);
        
        if (level == null) return new Response(HttpStatusCode.NotFound);

        return action switch
        {
            "delete" => LevelManagementEndpoints.RemoveLevel(dataStore, database, user, level),
            "latest" => new Response(new LevelResponse(level, user), ContentType.Json),
            _ => new Response(HttpStatusCode.NotFound)
        };
    }

    [GameEndpoint("~level:{args}", Method.Post)]
    public Response PostEndpoints(RequestContext context, IDataStore dataStore, ProfanityService profanity, Stream body, GameDatabaseContext database, GameServerConfig config, GameUser user, GameSession session, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];

        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        if (action == "create") return LevelManagementEndpoints.CreateLevel(context, config, dataStore, profanity, parser, database, user, session);
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);

        return action == "update" ? LevelManagementEndpoints.UpdateLevel(context, dataStore, profanity, parser, database, user, level.Id) : new Response(HttpStatusCode.NotFound);
    }
}
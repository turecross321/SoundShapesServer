using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Core.Storage;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using HttpMultipartParser;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
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

    [GameEndpoint("~level:{args}", HttpMethods.Post)]
    public Response PostEndpoints(RequestContext context, IDataStore dataStore, ProfanityService profanity, Stream body, GameDatabaseContext database, GameServerConfig config, GameUser user, GameToken token, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];

        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        if (action == "create") return LevelManagementEndpoints.CreateLevel(context, config, dataStore, profanity, parser, database, user, token);
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);

        return action == "update" ? LevelManagementEndpoints.UpdateLevel(context, dataStore, parser, database, user, level.Id) : new Response(HttpStatusCode.NotFound);
    }
}
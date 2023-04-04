using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelSpecificEndpoints : EndpointGroup
{
    [Endpoint("/otg/~level:{args}")]
    public static Response Endpoints(RequestContext context, RealmDatabaseContext database, GameUser user, string args)
    {
        string[] arguments = args.Split('.');

        string levelId = arguments[0];
        string action = arguments[1];
        
        GameLevel? level = database.GetLevelWithId(levelId);
        
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        // Doing this because Bunkum doesn't support . as a seperator
        if (action == "delete") return UnPublishLevel(database, user, level);

        else return new Response(HttpStatusCode.NotFound);
    }

    private static Response UnPublishLevel(RealmDatabaseContext database, GameUser user, GameLevel level)
    {
        if (level.author.Equals(user) == false) return new Response(HttpStatusCode.Forbidden);  // Check if user is the level publisher

        return database.UnPublishLevel(level) ? new Response(HttpStatusCode.OK) : HttpStatusCode.InternalServerError;
    }
}
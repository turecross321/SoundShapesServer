using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelRelationEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{userId}/~like:%2F~level%3A{arguments}", ContentType.Json)]
    public Response LevelLikeRequests(RequestContext context, GameDatabaseContext database, GameUser user, string userId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to seperate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string levelId = argumentArray[0];
        string requestType = argumentArray[1];

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        if (requestType == "put") return LikeLevel(user, level, database);
        if (requestType == "get") return CheckIfUserHasLikedLevel(user, level, database);
        if (requestType == "delete") return UnLikeLevel(user, level, database);

        return new Response(HttpStatusCode.NotFound);
    }

    private Response CheckIfUserHasLikedLevel(GameUser user, GameLevel level, GameDatabaseContext database)
    {
        if (database.IsUserLikingLevel(user, level))
        {
            // Returning an empty class because this doesn't actually need any data. It just needs a response and some sort of object
            LevelLikeResponse response = new();

            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }
    private Response LikeLevel(GameUser user, GameLevel level, GameDatabaseContext database)
    {
        if (database.LikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }

    private Response UnLikeLevel(GameUser user, GameLevel level, GameDatabaseContext database)
    {
        if (database.UnLikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }
}
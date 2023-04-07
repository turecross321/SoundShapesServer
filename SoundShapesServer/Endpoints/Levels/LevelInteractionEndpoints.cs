using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public static class LevelInteractionEndpoints
{
    // Called from LeaderboardEndpoints
    public static void AddCompletion(RealmDatabaseContext database, GameLevel level, GameUser user)
    {
        database.AddCompletionToLevel(level);
    }
    // Called from LeaderboardEndpoints
    public static void AddPlay(RealmDatabaseContext database, GameLevel level)
    {
        database.AddPlayToLevel(level);
    }

    // Called from LeaderboardEndpoints
    public static void AddUniquePlay(RealmDatabaseContext database, GameLevel level, GameUser user)
    {
        database.AddUniquePlayToLevel(level, user);
    }
    // Called from LeaderboardEndpoints
    public static void AddDeathsToLevel(RealmDatabaseContext database, GameLevel level, int deaths)
    {
        database.AddDeathsToLevel(level, deaths);
    }
    
    [Endpoint("/otg/~identity:{userId}/~like:%2F~level%3A{arguments}", ContentType.Json)]
    public static Response LevelLikeRequests(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string arguments)
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

    private static Response CheckIfUserHasLikedLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.IsUserLikingLevel(user, level))
        {
            // Returning an empty class because this doesn't actually need any data. It just needs a response and some sort of object
            LevelLikeResponse response = new LevelLikeResponse();

            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }
    private static Response LikeLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.LikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        else return new Response(HttpStatusCode.BadRequest);  
    }

    private static Response UnLikeLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.UnLikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        else return new Response(HttpStatusCode.BadRequest);  
    }
}
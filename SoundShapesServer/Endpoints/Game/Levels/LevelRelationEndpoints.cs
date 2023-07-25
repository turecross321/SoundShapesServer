using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelRelationEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{userId}/~queued:*.page")]
    public ListResponse<RelationLevelResponse>? QueuedAndLiked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);
        
        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) return null;
        
        (GameLevel[] levels, int totalLevels) = database.GetLevels(LevelOrderType.DoNotOrder, true, new LevelFilters(likedOrQueuedByUser: userToGetLevelsFrom), from, count);
        
        return new ListResponse<RelationLevelResponse>(levels.Select(l=>new RelationLevelResponse(l, user)), totalLevels, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~like:*.page")]
    public ListResponse<RelationLevelResponse>? Liked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context); 
        
        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) return null;
        
        (GameLevel[] levels, int totalLevels) = database.GetLevels(LevelOrderType.DoNotOrder, true, new LevelFilters(likedByUser: userToGetLevelsFrom), from, count);
        
        return new ListResponse<RelationLevelResponse>(levels.Select(l=>new RelationLevelResponse(l, user)), totalLevels, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~like:%2F~level%3A{arguments}")]
    public Response LevelLikeRequests(RequestContext context, GameDatabaseContext database, GameUser user, string userId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to separate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string levelId = argumentArray[0];
        string requestType = argumentArray[1];

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        if (requestType == "put") return LikeLevel(database, user, level);
        if (requestType == "get") return CheckIfUserHasLikedLevel(database, user, level);
        if (requestType == "delete") return UnLikeLevel(database, user, level);

        return new Response(HttpStatusCode.NotFound);
    }
    
    [GameEndpoint("~identity:{userId}/~queued:%2F~level%3A{arguments}")]
    public Response LevelQueueRequests(RequestContext context, GameDatabaseContext database, GameUser user, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to separate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string levelId = argumentArray[0];
        string requestType = argumentArray[1];
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        // There is no queue button, and this is always called when the like button is pressed, so ignore it.
        if (requestType == "put") return HttpStatusCode.NotFound;
        if (requestType == "get") return CheckIfUserHasQueuedLevel(database, user, level);
        if (requestType == "delete") return UnQueueLevel(database, user, level);

        return new Response(HttpStatusCode.NotFound);
    }

    private Response CheckIfUserHasLikedLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.HasUserLikedLevel(user, level))
        {
            LevelRelationResponse response = new();
            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }
    private Response LikeLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.LikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }

    private Response UnLikeLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.UnLikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }

    private Response CheckIfUserHasQueuedLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.HasUserQueuedLevel(user, level))
        {
            LevelRelationResponse response = new();
            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }

    private Response UnQueueLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.UnQueueLevel(user, level)) return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }
}
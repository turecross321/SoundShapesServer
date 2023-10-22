using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelRelationEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{userId}/~queued:*.page")]
    public ListResponse<RelationLevelResponse>? QueuedAndLiked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        (int from, int count, bool _) = context.GetPageData();
        
        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) 
            // game will get stuck if this isn't done
            return new ListResponse<RelationLevelResponse>();
        
        (LevelQueueRelation[] queued, int totalQueued) = userToGetLevelsFrom.QueuedLevelRelations.Paginate(from, count);
        (LevelLikeRelation[] liked, int totalLiked) = userToGetLevelsFrom.LikedLevelRelations.Paginate(from, count);
        // Combines queue relations and level relations into one RelationLevelResponse list
        IEnumerable<RelationLevelResponse> levels = queued.Select(r => new RelationLevelResponse(r, user))
            .Concat(liked.Select(r => new RelationLevelResponse(r, user)));
        
        return new ListResponse<RelationLevelResponse>(levels, totalQueued + totalLiked, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~like:*.page")]
    public ListResponse<RelationLevelResponse>? Liked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        (int from, int count, bool _) = context.GetPageData();
        
        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) 
            // game will get stuck if this isn't done
            return new ListResponse<RelationLevelResponse>();

        (LevelLikeRelation[] relations, int totalLevels) = userToGetLevelsFrom.LikedLevelRelations.Paginate(from, count);
        return new ListResponse<RelationLevelResponse>(relations.Select(r=>new RelationLevelResponse(r, user)), totalLevels, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~like:%2F~level%3A{arguments}")]
    public Response LevelLikeRequests(RequestContext context, GameDatabaseContext database, GameUser user, GameToken token, string userId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to separate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string levelId = argumentArray[0];
        string requestType = argumentArray[1];

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) 
            return new Response(HttpStatusCode.NotFound);
        
        if (requestType == "put") return LikeLevel(database, user, level, token.PlatformType);
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
        if (level == null) 
            return new Response(HttpStatusCode.NotFound);
        
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
    private Response LikeLevel(GameDatabaseContext database, GameUser user, GameLevel level, PlatformType platformType)
    {
        if (!level.HasUserAccess(user))
            return HttpStatusCode.NotFound;
        
        if (database.LikeLevel(user, level, platformType)) 
            return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.Conflict);
    }

    private Response UnLikeLevel(GameDatabaseContext database, GameUser user, GameLevel level)
    {
        if (database.UnLikeLevel(user, level)) 
            return new Response(HttpStatusCode.OK);
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
        if (!level.HasUserAccess(user))
            return HttpStatusCode.NotFound;
        
        if (database.UnQueueLevel(user, level)) 
            return new Response(HttpStatusCode.OK);
        return new Response(HttpStatusCode.BadRequest);
    }
}
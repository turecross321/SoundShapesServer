using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelRelationEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{levelId}/users/id/{userId}", ContentType.Json)]
    [Authentication(false)]
    public ApiLevelRelationResponse? GetLevelRelation(RequestContext context, GameDatabaseContext database, string levelId, string userId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;

        return new ApiLevelRelationResponse()
        {
            Completed = level.UniqueCompletions.Contains(user),
            Liked = database.HasUserLikedLevel(user, level),
            Queued = database.HasUserQueuedLevel(user, level)
        };
    }

    [ApiEndpoint("levels/id/{id}/like", Method.Post)]
    public Response LikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.LikeLevel(user, level)) return HttpStatusCode.Created;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("levels/id/{id}/unLike", Method.Post)]
    public Response UnLikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.UnLikeLevel(user, level)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
    
    [ApiEndpoint("levels/id/{id}/queue", Method.Post)]
    public Response QueueLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.QueueLevel(user, level)) return HttpStatusCode.Created;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("levels/id/{id}/unQueue", Method.Post)]
    public Response UnQueueLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (database.UnQueueLevel(user, level)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
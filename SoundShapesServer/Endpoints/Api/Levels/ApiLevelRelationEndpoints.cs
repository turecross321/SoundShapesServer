using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelRelationEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{levelId}/relationWith/id/{userId}"), Authentication(false)]
    [DocSummary("Retrieves relation between level and user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocRouteParam("levelId", "Level ID.")]
    //todo: [DocRouteParam("userId", "User ID.")]
    public ApiResponse<ApiLevelRelationResponse> GetLevelRelation(RequestContext context, GameDatabaseContext database, string levelId, string userId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null)
            return ApiNotFoundError.UserNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;

        return new ApiLevelRelationResponse
        {
            Completed = level.UniqueCompletions.Contains(user),
            Liked = database.HasUserLikedLevel(user, level),
            Queued = database.HasUserQueuedLevel(user, level)
        };
    }

    [ApiEndpoint("levels/id/{id}/like", HttpMethods.Post)]
    [DocSummary("Likes level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.AlreadyLikedLevelWhen)]
    public ApiOkResponse LikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;
        
        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;

        if (!database.LikeLevel(user, level, PlatformType.Unknown)) 
            return ApiConflictError.AlreadyLikedLevel;
        
        return new ApiOkResponse();
    }

    [ApiEndpoint("levels/id/{id}/unLike", HttpMethods.Post)]
    [DocSummary("Removes like on level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NotLikedLevelWhen)]
    public ApiOkResponse UnLikeLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;
        
        if (!database.UnLikeLevel(user, level))
            return ApiNotFoundError.NotLikedLevel;
        
        return new ApiOkResponse();
    }
    
    [ApiEndpoint("levels/id/{id}/queue", HttpMethods.Post)]
    [DocSummary("Queues level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.AlreadyQueuedLevelWhen)]
    public ApiOkResponse QueueLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;
        
        if (!database.QueueLevel(user, level, PlatformType.Unknown)) 
            return ApiConflictError.AlreadyQueuedLevel;

        return new ApiOkResponse();
    }

    [ApiEndpoint("levels/id/{id}/unQueue", HttpMethods.Post)]
    [DocSummary("Removes queue on level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NotQueuedLevelWhen)]
    public ApiOkResponse UnQueueLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;
        
        if (!database.UnQueueLevel(user, level))         
            return ApiNotFoundError.NotQueuedLevel;
        
        return new ApiOkResponse();
    }
}
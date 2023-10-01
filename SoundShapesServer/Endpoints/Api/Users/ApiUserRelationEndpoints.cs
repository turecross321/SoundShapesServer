using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserRelationEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{recipientId}/relationWith/id/{actorId}"), Authentication(false)]
    [DocSummary("Retrieves relation between two users.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    public ApiResponse<ApiUserRelationResponse> CheckIfFollowingUser(RequestContext context, GameDatabaseContext database, string recipientId, string actorId)
    {
        GameUser? recipient = database.GetUserWithId(recipientId);
        if (recipient == null)
            return ApiNotFoundError.UserNotFound;
        
        GameUser? actor = database.GetUserWithId(actorId);
        if (actor == null) 
            return ApiNotFoundError.UserNotFound;

        return new ApiUserRelationResponse
        {
            Following = database.IsUserFollowingOtherUser(actor, recipient),
            Followed = database.IsUserFollowingOtherUser(recipient, actor)
        };
    }

    [ApiEndpoint("users/id/{id}/follow", HttpMethods.Post)]
    [DocSummary("Follows user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiForbiddenError), ApiForbiddenError.FollowYourselfWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.AlreadyFollowingWhen)]
    public ApiOkResponse FollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null)
            return ApiNotFoundError.UserNotFound;
        
        if (recipient.Id == user.Id) 
            return ApiForbiddenError.FollowYourself;

        if (!database.FollowUser(user, recipient)) 
            return ApiConflictError.AlreadyFollowing;
        
        return new ApiOkResponse();
    }

    [ApiEndpoint("users/id/{id}/unFollow", HttpMethods.Post)]
    [DocSummary("Unfollows user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiForbiddenError), ApiNotFoundError.NotFollowingWhen)]
    public ApiOkResponse UnFollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null)
            return ApiNotFoundError.UserNotFound;

        if (!database.UnFollowUser(user, recipient)) 
            return ApiNotFoundError.NotFollowing;
        
        return new ApiOkResponse();
    }
}
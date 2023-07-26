using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserRelationEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{recipientId}/relationWith/id/{actorId}"), Authentication(false)]
    [DocSummary("Retrieves relation between two users.")]
    [DocError(typeof(NotFoundError), NotFoundError.UserNotFoundWhen)]
    public ApiUserRelationResponse? CheckIfFollowingUser(RequestContext context, GameDatabaseContext database, string recipientId, string actorId)
    {
        GameUser? recipient = database.GetUserWithId(recipientId);
        if (recipient == null) return null;
        
        GameUser? actor = database.GetUserWithId(actorId);
        if (actor == null) return null;

        return new ApiUserRelationResponse
        {
            Following = database.IsUserFollowingOtherUser(actor, recipient),
            Followed = database.IsUserFollowingOtherUser(recipient, actor)
        };
    }

    [ApiEndpoint("users/id/{id}/follow", Method.Post)]
    [DocSummary("Follows user with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ForbiddenError), ForbiddenError.FollowYourselfWhen)]
    public Response FollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        
        if (recipient.Id == user.Id) 
            return new Response(ForbiddenError.FollowYourselfWhen, ContentType.Plaintext, HttpStatusCode.Forbidden);

        if (!database.FollowUser(user, recipient)) 
            return new Response(ConflictError.AlreadyFollowingWhen, ContentType.Plaintext, HttpStatusCode.Conflict);
        
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("users/id/{id}/unFollow", Method.Post)]
    [DocSummary("Unfollows user with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.UserNotFoundWhen)]
    [DocError(typeof(NotFoundError), NotFoundError.NotFollowingWhen)]
    public Response UnFollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;

        if (!database.UnFollowUser(user, recipient)) 
            return new Response(NotFoundError.NotFollowingWhen, ContentType.Plaintext, HttpStatusCode.NotFound);
        
        return HttpStatusCode.OK;
    }
}
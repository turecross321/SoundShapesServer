using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserRelationEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{recipientId}/users/id/{actorId}")]
    [Authentication(false)]
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
    public Response FollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Id == user.Id) return HttpStatusCode.Forbidden;

        if (database.FollowUser(user, recipient)) return HttpStatusCode.Created;
        
        return HttpStatusCode.Conflict;
    }

    [ApiEndpoint("users/id/{id}/unFollow", Method.Post)]
    public Response UnFollowUser(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return HttpStatusCode.NotFound;
        if (recipient.Id == user.Id) return HttpStatusCode.Forbidden;

        if (database.UnFollowUser(user, recipient)) return HttpStatusCode.OK;
        
        return HttpStatusCode.Conflict;
    }
}
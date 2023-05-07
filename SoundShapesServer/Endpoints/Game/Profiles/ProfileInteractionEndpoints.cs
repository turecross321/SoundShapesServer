using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Following;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Profiles;

public class ProfileInteractionEndpoints : EndpointGroup
{

    [GameEndpoint("~identity:{followerId}/~follow:%2F~identity%3A{arguments}", ContentType.Json)]
    public Response FollowRequests(RequestContext context, GameDatabaseContext database, GameUser user, string followerId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to separate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string userId = argumentArray[0];
        string requestType = argumentArray[1];

        GameUser? recipient = database.GetUserWithId(userId);
        if (recipient == null) return new Response(HttpStatusCode.NotFound);

        return requestType switch
        {
            "get" => new Response(CheckIfUserIsFollowed(user, recipient, database), ContentType.Json),
            "put" => FollowUser(user, recipient, database),
            "delete" => UnFollowUser(user, recipient, database),
            _ => new Response(HttpStatusCode.NotFound)
        };
    }
    
    private FollowResponse? CheckIfUserIsFollowed(GameUser follower, GameUser recipient, GameDatabaseContext database)
    {
        if (database.IsUserFollowingOtherUser(follower, recipient))
        {
            return new FollowResponse(recipient);
        }

        return null;
    }
    private Response FollowUser(GameUser follower, GameUser recipient, GameDatabaseContext database)
    {
        return database.FollowUser(follower, recipient) ? new Response(HttpStatusCode.Created) : new Response(HttpStatusCode.BadRequest);
    }

    private Response UnFollowUser(GameUser user, GameUser recipient, GameDatabaseContext database)
    {
        return database.UnFollowUser(user, recipient) ? new Response(HttpStatusCode.OK) : new Response(HttpStatusCode.BadRequest);
    }
}
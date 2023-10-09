using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Users;

public class UserInteractionEndpoints : EndpointGroup
{

    [GameEndpoint("~identity:{followerId}/~follow:%2F~identity%3A{arguments}")]
    public Response FollowRequests(RequestContext context, GameDatabaseContext database, GameUser user, AuthToken token, string followerId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to separate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string userId = argumentArray[0];
        string requestType = argumentArray[1];

        GameUser? recipient = database.GetUserWithId(userId);
        if (recipient == null) return new Response(HttpStatusCode.NotFound);

        return requestType switch
        {
            "get" => new Response(CheckIfUserIsFollowed(user, recipient, database), ContentType.Json),
            "put" => FollowUser(user, recipient, database, token.PlatformType),
            "delete" => UnFollowUser(user, recipient, database),
            _ => new Response(HttpStatusCode.NotFound)
        };
    }
    
    private UserFullResponse? CheckIfUserIsFollowed(GameUser follower, GameUser recipient, GameDatabaseContext database)
    {
        if (database.IsUserFollowingOtherUser(follower, recipient))
        {
            return new UserFullResponse(recipient);
        }

        return null;
    }
    private Response FollowUser(GameUser follower, GameUser recipient, GameDatabaseContext database, PlatformType platformType)
    {
        return database.FollowUser(follower, recipient, platformType) ? new Response(HttpStatusCode.Created) : new Response(HttpStatusCode.BadRequest);
    }

    private Response UnFollowUser(GameUser user, GameUser recipient, GameDatabaseContext database)
    {
        return database.UnFollowUser(user, recipient) ? new Response(HttpStatusCode.OK) : new Response(HttpStatusCode.BadRequest);
    }
}
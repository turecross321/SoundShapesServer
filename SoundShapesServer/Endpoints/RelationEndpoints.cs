using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints;

public class RelationEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{userId}/~like:%2F~level%3A{arguments}", ContentType.Json)]
    public Response LevelLikeRequests(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string arguments)
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

    private Response CheckIfUserHasLikedLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.IsUserLikingLevel(user, level))
        {
            // Returning an empty class because this doesn't actually need any data. It just needs a response and some sort of object
            LevelLikeResponse response = new LevelLikeResponse();

            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }
    private Response LikeLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.LikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        else return new Response(HttpStatusCode.BadRequest);  
    }

    private Response UnLikeLevel(GameUser user, GameLevel level, RealmDatabaseContext database)
    {
        if (database.UnLikeLevel(user, level)) return new Response(HttpStatusCode.OK);
        else return new Response(HttpStatusCode.BadRequest);  
    }
    
    
    [Endpoint("/otg/~identity:{followerId}/~follow:%2F~identity%3A{arguments}", ContentType.Json)]
    public Response FollowRequests(RequestContext context, RealmDatabaseContext database, GameUser user, string followerId, string arguments)
    {
        string[] argumentArray = arguments.Split("."); // This is to seperate the .put / .get / delete from the id, which Bunkum currently cannot do by it self
        string userId = argumentArray[0];
        string requestType = argumentArray[1];

        GameUser? followedUser = database.GetUserWithId(userId);
        if (followedUser == null) return new Response(HttpStatusCode.NotFound);
        
        if (requestType == "put") return FollowUser(user, followedUser, database);
        if (requestType == "get") return CheckIfUserIsFollowed(user, followedUser, database);
        if (requestType == "delete") return UnFollowUser(user, followedUser, database);

        return new Response(HttpStatusCode.NotFound);
    }
    
    private Response CheckIfUserIsFollowed(GameUser user, GameUser userBeingFollowed, RealmDatabaseContext database)
    {
        if (database.IsUserFollowingOtherUser(user, userBeingFollowed))
        {
            FollowResponse response = new()
            {
                id = userBeingFollowed.id,
                type = ResponseType.follow.ToString(),
                target = new RelationTarget()
                {
                    id = IdFormatter.FormatUserId(userBeingFollowed.id),
                    type = ResponseType.identity.ToString(),
                    displayName = userBeingFollowed.display_name
                }
            };

            return new Response(response, ContentType.Json);
        }

        return new Response(HttpStatusCode.NotFound);
    }
    private Response FollowUser(GameUser follower, GameUser userBeingFollowed, RealmDatabaseContext database)
    {
        if (database.FollowUser(follower, userBeingFollowed)) return new Response(HttpStatusCode.Created);
        else return new Response(HttpStatusCode.BadRequest);  
    }

    private Response UnFollowUser(GameUser user, GameUser userBeingUnFollowed, RealmDatabaseContext database)
    {
        if (database.UnFollowUser(user, userBeingUnFollowed)) return new Response(HttpStatusCode.OK);
        else return new Response(HttpStatusCode.BadRequest);  
    }
}
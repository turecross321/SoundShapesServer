using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}")]
    [Authentication(false)]
    public ApiUserResponse? GetUser(RequestContext context, RealmDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserResponse(userToCheck);
    }

    [ApiEndpoint("users")]
    [Authentication(false)]
    public ApiUsersWrapper? GetUsers(RequestContext context, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];
        string category = context.QueryString["category"] ?? "registered";

        IQueryable<GameUser>? users = null;
        
        string? userId = context.QueryString["userId"];
        switch (category)
        {
            case "registered":
            {
                users = database.GetUsers().Where(u => u.HasFinishedRegistration);
                break;
            }
            case "following":
            {
                if (userId == null) return null;

                GameUser? followingUsers = database.GetUserWithId(userId);
                if (followingUsers == null) return null;

                users = followingUsers.Following.Select(r=>r.Recipient);
                break;
            }
            case "followers":
                if (userId == null) return null;

                GameUser? followedUsers = database.GetUserWithId(userId);
                if (followedUsers == null) return null;

                users = followedUsers.Followers.Select(r=>r.Follower);
                break;
        }
        
        users ??= database.GetUsers();
        
        UserOrderType order = orderString switch
        {
            "followerCount" => UserOrderType.FollowerCount,
            "followingCount" => UserOrderType.FollowingCount,
            "levelCount" => UserOrderType.LevelCount,
            "creationDate" => UserOrderType.CreationDate,
            "playedLevelsCount" => UserOrderType.PlayedLevelsCount,
            "leaderboardPlacements" => UserOrderType.LeaderboardPlacements,
            _ => UserOrderType.CreationDate
        };

        return new ApiUsersWrapper(database, users, from, count, order, descending);
    }
}
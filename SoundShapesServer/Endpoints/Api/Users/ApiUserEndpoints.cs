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

        bool registered = bool.Parse(context.QueryString["registered"] ?? "true");
        
        string? whoIsFollowing = context.QueryString["whoIsFollowing"]; 
        string? whoIsFollowedBy = context.QueryString["whoIsFollowedBy"]; 

        IQueryable<GameUser> users = database.GetUsers();

        if (registered)
        {
            users = users
                .AsEnumerable()
                .Where(u => u.HasFinishedRegistration)
                .AsQueryable();
        }

        if (whoIsFollowing != null)
        {
            GameUser? userToGetUsersFrom = database.GetUserWithId(whoIsFollowing);
            if (userToGetUsersFrom == null) return null;

            users = users
                .AsEnumerable()
                .Where(u => userToGetUsersFrom.Followers
                    .Select(r => r.Follower.Id).Contains(u.Id))
                .AsQueryable();
        }
        
        if (whoIsFollowedBy != null)
        {
            GameUser? userToGetUsersFrom = database.GetUserWithId(whoIsFollowing);
            if (userToGetUsersFrom == null) return null;

            users = users
                .AsEnumerable()
                .Where(u => userToGetUsersFrom.Following
                    .Select(r => r.Recipient.Id).Contains(u.Id))
                .AsQueryable();
        }

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
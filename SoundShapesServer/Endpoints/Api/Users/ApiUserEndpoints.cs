using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/{id}")]
    [Authentication(false)]
    public ApiUserResponse? GetUser(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserResponse(userToCheck);
    }

    [ApiEndpoint("users")]
    [Authentication(false)]
    public ApiUsersWrapper? GetUsers(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? following = context.QueryString["following"]; 
        string? followedBy = context.QueryString["followedBy"];

        string? search = context.QueryString["search"];

        IQueryable<GameUser>? users = null;

        if (search != null) users = database.SearchForUsers(search);
        users ??= database.GetUsers();
        
        users = UserHelper.FilterUsers(database, users, following, followedBy);
        if (users == null) return null;

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
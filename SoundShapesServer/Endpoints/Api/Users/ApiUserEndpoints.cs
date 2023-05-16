using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}")]
    [Authentication(false)]
    public ApiUserFullResponse? GetUserWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }
    
    [ApiEndpoint("users/username/{username}")]
    [Authentication(false)]
    public ApiUserFullResponse? GetUserWithUsername(RequestContext context, GameDatabaseContext database, string username)
    {
        GameUser? userToCheck = database.GetUserWithUsername(username);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }

    [ApiEndpoint("users")]
    [Authentication(false)]
    public ApiUsersWrapper GetUsers(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? isFollowingId = context.QueryString["isFollowing"]; 
        string? followedById = context.QueryString["followedBy"];

        string? searchQuery = context.QueryString["search"];
        
        GameUser? isFollowing = null;
        GameUser? followedBy = null;
        
        if (isFollowingId != null) isFollowing = database.GetUserWithId(isFollowingId);
        if (followedById != null) followedBy = database.GetUserWithId(followedById);

        UserFilters filters = new (isFollowing, followedBy, searchQuery);

        UserOrderType order = orderString switch
        {
            "followersCount" => UserOrderType.FollowersCount,
            "followingCount" => UserOrderType.FollowingCount,
            "levelsCount" => UserOrderType.LevelsCount,
            "likedLevelsCount" => UserOrderType.LikedLevelsCount,
            "creationDate" => UserOrderType.CreationDate,
            "playedLevelsCount" => UserOrderType.PlayedLevelsCount,
            "completedLevelsCount" => UserOrderType.CompletedLevelsCount,
            "deaths" => UserOrderType.Deaths,
            "totalPlayTime" => UserOrderType.TotalPlayTime,
            _ => UserOrderType.CreationDate
        };

        (GameUser[] users, int totalUsers) = database.GetUsers(order, descending, filters, from, count);
        return new ApiUsersWrapper(users, totalUsers);
    }
}
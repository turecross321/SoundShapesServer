using Bunkum.HttpServer;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Users;
using static System.Text.RegularExpressions.Regex;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    private const string UsernameRegex = "^[A-Za-z][A-Za-z0-9-_]{2,15}$";
    public static bool IsUsernameLegal(string username)
    {
        return IsMatch(username, UsernameRegex);
    }

    public static UserOrderType GetUserOrderType(RequestContext context)
    {
        string? orderString = context.QueryString["orderBy"];
        
        return orderString switch
        {
            "followers" => UserOrderType.Followers,
            "following" => UserOrderType.Following,
            "publishedLevels" => UserOrderType.PublishedLevels,
            "likedLevels" => UserOrderType.LikedLevels,
            "creationDate" => UserOrderType.CreationDate,
            "playedLevels" => UserOrderType.PlayedLevels,
            "completedLevels" => UserOrderType.CompletedLevels,
            "totalDeaths" => UserOrderType.Deaths,
            "totalPlayTime" => UserOrderType.TotalPlayTime,
            "lastGameLogin" => UserOrderType.LastGameLogin,
            _ => UserOrderType.CreationDate
        };
    }

    public static UserFilters GetUserFilters(RequestContext context, GameDatabaseContext database)
    {
        string? isFollowingId = context.QueryString["isFollowing"]; 
        string? followedById = context.QueryString["followedBy"];

        string? searchQuery = context.QueryString["search"];
        
        GameUser? isFollowing = null;
        GameUser? followedBy = null;
        
        if (isFollowingId != null) isFollowing = database.GetUserWithId(isFollowingId);
        if (followedById != null) followedBy = database.GetUserWithId(followedById);

        return new UserFilters(isFollowing, followedBy, searchQuery);
    }
}
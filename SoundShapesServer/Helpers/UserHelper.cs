using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static partial class UserHelper
{
    [System.Text.RegularExpressions.GeneratedRegex("^[A-Za-z][A-Za-z0-9-_]{2,15}$")]
    private static partial System.Text.RegularExpressions.Regex UsernameRegex();
    public static bool IsUsernameLegal(string username)
    {
        return UsernameRegex().IsMatch(username);
    }
    
    public static UserOrderType GetUserOrderType(this RequestContext context)
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
            "events" => UserOrderType.Events,
            _ => UserOrderType.CreationDate
        };
    }

    public static UserFilters GetUserFilters(RequestContext context, GameDatabaseContext database)
    {
        GameUser? isFollowing = context.QueryString["isFollowing"].ToUser(database);
        GameUser? followedBy = context.QueryString["followedBy"].ToUser(database);
        string? searchQuery = context.QueryString["search"];

        return new UserFilters(isFollowing, followedBy, searchQuery);
    }
}
using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class UserRequestContextExtensions
{
    public static UserOrderType GetUserOrderType(this RequestContext context)
    {
        return context.QueryString["orderBy"] switch
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

    public static UserFilters GetUserFilters(this RequestContext context, GameDatabaseContext database)
    {
        return new UserFilters
        {
            IsFollowingUser = context.QueryString["isFollowing"].ToUser(database),
            FollowedByUser = context.QueryString["followedBy"].ToUser(database),
            Search = context.QueryString["search"],
            Deleted = false,
            CreatedBefore = context.QueryString["createdBefore"].ToDateFromUnix(),
            CreatedAfter = context.QueryString["createdAfter"].ToDateFromUnix(),
        };
    }
}
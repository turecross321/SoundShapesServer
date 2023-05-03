using SoundShapesServer.Database;
using SoundShapesServer.Types;
using static System.Text.RegularExpressions.Regex;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    private const string UsernameRegex = "^[A-Za-z][A-Za-z0-9-_]{2,15}$";
    public static bool IsUsernameLegal(string username)
    {
        return IsMatch(username, UsernameRegex);
    }

    public static IQueryable<GameUser>? FilterUsers(RealmDatabaseContext database, IQueryable<GameUser> users, string? following, string? followedBy)
    {
        IQueryable<GameUser> response = users;
        
        if (following != null)
        {
            GameUser? userToGetUsersFrom = database.GetUserWithId(following);
            if (userToGetUsersFrom == null) return null;

            response = response
                .AsEnumerable()
                .Where(u => userToGetUsersFrom.Followers
                    .Select(r => r.Follower.Id).Contains(u.Id))
                .AsQueryable();
        }
        
        if (followedBy != null)
        {
            GameUser? userToGetUsersFrom = database.GetUserWithId(following);
            if (userToGetUsersFrom == null) return null;

            response = response
                .AsEnumerable()
                .Where(u => userToGetUsersFrom.Following
                    .Select(r => r.Recipient.Id).Contains(u.Id))
                .AsQueryable();
        }

        return response;
    }

    public static IQueryable<GameUser> OrderUsers(RealmDatabaseContext database, IQueryable<GameUser> users, UserOrderType order)
    {
        return order switch
        {
            UserOrderType.FollowerCount => users.OrderBy(u=>u.Followers.Count()),
            UserOrderType.FollowingCount => users.OrderBy(u=>u.Following.Count()),
            UserOrderType.LevelCount => users.OrderBy(u=>u.Levels.Count()),
            UserOrderType.CreationDate => users.OrderBy(u=>u.CreationDate),
            UserOrderType.PlayedLevelsCount => users.OrderBy(u=>u.PlayedLevels.Count()),
            UserOrderType.LeaderboardPlacements => users.OrderByDescending(u=> LeaderboardHelper.GetTotalLeaderboardPlacements(database, u)),
            UserOrderType.DoNotOrder => users,
            _ => OrderUsers(database, users, UserOrderType.CreationDate)
        };
    }
}
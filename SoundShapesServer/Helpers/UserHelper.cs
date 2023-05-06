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
        IQueryable<GameUser> response = users.Where(u=>u.HasFinishedRegistration);
        
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

    public static IQueryable<GameUser> OrderUsers(RealmDatabaseContext database, IQueryable<GameUser> users, UserOrderType order, bool descending)
    {
        IQueryable<GameUser> response = users;

        response = order switch
        {
            UserOrderType.FollowerCount => response.OrderBy(u=>u.Followers.Count()),
            UserOrderType.FollowingCount => response.OrderBy(u=>u.Following.Count()),
            UserOrderType.LevelCount => response.OrderBy(u=>u.Levels.Count()),
            UserOrderType.CreationDate => response.OrderBy(u=>u.CreationDate),
            UserOrderType.PlayedLevelsCount => response.OrderBy(u=>u.PlayedLevels.Count()),
            UserOrderType.LeaderboardPlacements => response.OrderByDescending(u=> LeaderboardHelper.GetTotalLeaderboardPlacements(database, u)),
            UserOrderType.DoNotOrder => response,
            _ => OrderUsers(database, response, UserOrderType.CreationDate, descending)
        };

        if (descending) response = response.AsEnumerable().Reverse().AsQueryable();

        return response;
    }
}
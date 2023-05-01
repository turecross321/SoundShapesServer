using System.Text.RegularExpressions;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    private const string UsernameRegex = "^[A-Za-z][A-Za-z0-9-_]{2,15}$";
    public static bool IsUsernameLegal(string username)
    {
        return Regex.IsMatch(username, UsernameRegex);
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
            _ => users.OrderBy(u=>u.CreationDate)
        };
    }
}
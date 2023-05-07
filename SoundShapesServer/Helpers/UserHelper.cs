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

    public static IQueryable<GameUser> FilterUsers(IQueryable<GameUser> users, UserFilters filters)
    {
        IQueryable<GameUser> response = users.Where(u=>u.HasFinishedRegistration);
        
        if (filters.IsFollowing != null)
        {
            response = response
                .AsEnumerable()
                .Where(u => filters.IsFollowing.Followers
                    .Select(r => r.Follower.Id).Contains(u.Id))
                .AsQueryable();
        }
        
        if (filters.FollowedBy != null)
        {
            response = response
                .AsEnumerable()
                .Where(u => filters.FollowedBy.Following
                    .Select(r => r.Recipient.Id).Contains(u.Id))
                .AsQueryable();
        }
        
        if (filters.Search != null)
        {
            response = response
                .AsEnumerable()
                .Where(l => l.Username.Contains(filters.Search, StringComparison.OrdinalIgnoreCase))
                .AsQueryable();
        }

        return response;
    }
}
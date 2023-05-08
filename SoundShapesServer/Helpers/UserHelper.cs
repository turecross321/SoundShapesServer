using SoundShapesServer.Types.Relations;
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
        
        if (filters.IsFollowingUser != null)
        {
            IQueryable<FollowRelation> relations = filters.IsFollowingUser.Followers;

            List<GameUser> tempResponse = new ();

            foreach (FollowRelation relation in relations)
            {
                GameUser follower = relation.Follower;
                GameUser? responseUser = response.FirstOrDefault(u => u.Id == follower.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.FollowedByUser != null)
        {
            IQueryable<FollowRelation> relations = filters.FollowedByUser.Following;

            List<GameUser> tempResponse = new ();

            foreach (FollowRelation relation in relations)
            {
                GameUser recipient = relation.Recipient;
                GameUser? responseUser = response.FirstOrDefault(u => u.Id == recipient.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.Search != null)
        {
            response = response.Where(l => l.Username.Contains(filters.Search, StringComparison.OrdinalIgnoreCase));
        }

        return response;
    }
}
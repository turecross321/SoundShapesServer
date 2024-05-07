using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.Queryable;

public static class UserQueryableExtensions
{
    public static IQueryable<GameUser> FilterUsers(this IQueryable<GameUser> users, UserFilters filters)
    {
        if (filters.CreatedBefore != null)
            users = users.Where(l => l.CreationDate <= filters.CreatedBefore);
        
        if (filters.CreatedAfter != null)
            users = users.Where(l => l.CreationDate >= filters.CreatedAfter);
        
        if (filters.IsFollowingUser != null)
        {
            IQueryable<UserFollowRelation> relations = filters.IsFollowingUser.FollowersRelations;

            List<GameUser> tempResponse = new();

            foreach (UserFollowRelation relation in relations)
            {
                GameUser follower = relation.Follower;
                GameUser? responseUser = users.FirstOrDefault(u => u.Id == follower.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            users = tempResponse.AsQueryable();
        }
        
        if (filters.FollowedByUser != null)
        {
            IQueryable<UserFollowRelation> relations = filters.FollowedByUser.FollowingRelations;

            List<GameUser> tempResponse = new();

            foreach (UserFollowRelation relation in relations)
            {
                GameUser recipient = relation.Recipient;
                GameUser? responseUser = users.FirstOrDefault(u => u.Id == recipient.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            users = tempResponse.AsQueryable();
        }
        
        if (filters.Search != null)
        {
            users = users.Where(u => u.Username.Contains(filters.Search, StringComparison.OrdinalIgnoreCase));
        }

        users = users.Where(u => u.Deleted == filters.Deleted);

        return users;
    }

    public static IQueryable<GameUser> OrderUsers(this IQueryable<GameUser> users, UserOrderType order, bool descending)
    {
        return order switch
        {
            UserOrderType.Followers => users.OrderByDynamic(u => u.FollowersCount, descending),
            UserOrderType.Following => users.OrderByDynamic(u => u.FollowingCount, descending),
            UserOrderType.PublishedLevels => users.OrderByDynamic(u => u.LevelsCount, descending),
            UserOrderType.LikedLevels => users.OrderByDynamic(u => u.LikedLevelsCount, descending),
            UserOrderType.CreationDate => users.OrderByDynamic(u => u.CreationDate, descending),
            UserOrderType.PlayedLevels => users.OrderByDynamic(u => u.PlayedLevelsCount, descending),
            UserOrderType.CompletedLevels => users.OrderByDynamic(u => u.CompletedLevelsCount, descending),
            UserOrderType.Deaths => users.OrderByDynamic(u => u.Deaths, descending),
            UserOrderType.PlayTime => users.OrderByDynamic(u => u.TotalPlayTime, descending),
            UserOrderType.LastGameLogin => users.OrderByDynamic(u => u.LastGameLogin, descending),
            UserOrderType.Events => users.OrderByDynamic(u => u.EventsCount, descending),
            _ => users.OrderUsers(UserOrderType.CreationDate, descending)
        };
    }
}
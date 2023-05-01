using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUsersWrapper
{
    public ApiUsersWrapper(RealmDatabaseContext database, IQueryable<GameUser> users, int from, int count, UserOrderType order, bool descending)
    {
        IQueryable<GameUser> orderedUsers = UserHelper.OrderUsers(database, users, order);
        IQueryable<GameUser> fullyOrderedUsers = descending ? orderedUsers
            .AsEnumerable()
            .Reverse()
            .AsQueryable() : orderedUsers;
        
        GameUser[] paginatedUsers = PaginationHelper.PaginateUsers(fullyOrderedUsers, from, count);
        
        Users = paginatedUsers.Select(u => new ApiUserResponse(u)).ToArray();
        Count = users.Count();
    }

    public ApiUserResponse[] Users { get; set; }
    public int Count { get; set; }
}
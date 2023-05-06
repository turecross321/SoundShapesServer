using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.UserHelper;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUsersWrapper
{
    public ApiUsersWrapper(RealmDatabaseContext database, IQueryable<GameUser> users, int from, int count, UserOrderType order, bool descending)
    {
        IQueryable<GameUser> orderedUsers = OrderUsers(database, users, order, descending);

        GameUser[] paginatedUsers = PaginationHelper.PaginateUsers(orderedUsers, from, count);
        
        Users = paginatedUsers.Select(u => new ApiUserResponse(u)).ToArray();
        Count = users.Count();
    }

    public ApiUserResponse[] Users { get; set; }
    public int Count { get; set; }
}
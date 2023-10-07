using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Users;

public class UserEndpoints : EndpointGroup
{
    [GameEndpoint("~index:identity.page")]
    public ListResponse<UserBriefResponse> GetUsers(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        UserFilters filters = UserHelper.GetUserFilters(context, database);
        UserOrderType order = UserHelper.GetUserOrderType(context);

        (GameUser[] users, int totalUsers) = database.GetPaginatedUsers(order, descending, filters, from, count);
        return new ListResponse<UserBriefResponse>(users.Select(u => new UserBriefResponse(user, u)), totalUsers, from, count);
    }
    
    [GameEndpoint("~identity:{id}/~metadata:*.get")]
    public UserMetadataResponse? GetUser(RequestContext context, string id, GameDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);
        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page")]
    public ListResponse<UserBriefResponse>? GetFollowing(RequestContext context, string id, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);

        GameUser? followingUser = database.GetUserWithId(id);
        if (followingUser == null) return null;

        (GameUser[] users, int totalUsers) = database.GetPaginatedUsers(UserOrderType.DoNotOrder, true, new UserFilters(followedByUser:followingUser), from, count);
        return new ListResponse<UserBriefResponse>(users.Select(u=>new UserBriefResponse(user, u)), totalUsers, from, count);
    }

    [GameEndpoint("~identity:{id}/~followers.page")]
    public ListResponse<UserBriefResponse>? GetFollowers(RequestContext context, string id, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);

        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;
        
        (GameUser[] users, int totalUsers) = database.GetPaginatedUsers(UserOrderType.DoNotOrder, true, new UserFilters(isFollowingUser:recipient), from, count);
        return new ListResponse<UserBriefResponse>(users.Select(u => new UserBriefResponse(user, u)), totalUsers, from, count);
    }
}
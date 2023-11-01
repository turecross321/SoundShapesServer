using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Users;

public class UserEndpoints : EndpointGroup
{
    [GameEndpoint("~index:identity.page")]
    public ListResponse<UserBriefResponse> GetUsers(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        UserFilters filters = context.GetFilters<UserFilters>(database);
        UserOrderType order = context.GetOrderType<UserOrderType>() ?? UserOrderType.CreationDate;

        PaginatedList<GameUser> users = database.GetPaginatedUsers(order, descending, filters, from, count);
        return new ListResponse<UserBriefResponse>(users.Items.Select(u => new UserBriefResponse(user, u)),
            users.TotalItems, from,
            count);
    }

    [GameEndpoint("~identity:{id}/~metadata:*.get")]
    public UserMetadataResponse? GetUser(RequestContext context, string id, GameDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);
        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page")]
    public ListResponse<UserBriefResponse> GetFollowing(RequestContext context, string id, GameDatabaseContext database,
        GameUser user)
    {
        (int from, int count, bool _) = context.GetPageData();

        GameUser? followedByUser = database.GetUserWithId(id);
        if (followedByUser == null)
            // game will get stuck if this isn't done
            return new ListResponse<UserBriefResponse>();

        PaginatedList<GameUser> users = database.GetPaginatedUsers(UserOrderType.DoNotOrder, true,
            new UserFilters { FollowedByUser = followedByUser }, from, count);
        return new ListResponse<UserBriefResponse>(users.Items.Select(u => new UserBriefResponse(user, u)),
            users.TotalItems, from,
            count);
    }

    [GameEndpoint("~identity:{id}/~followers.page")]
    public ListResponse<UserBriefResponse> GetFollowers(RequestContext context, string id, GameDatabaseContext database,
        GameUser user)
    {
        (int from, int count, bool _) = context.GetPageData();

        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null)
            // game will get stuck if this isn't done
            return new ListResponse<UserBriefResponse>();

        PaginatedList<GameUser> users = database.GetPaginatedUsers(UserOrderType.DoNotOrder, true,
            new UserFilters { IsFollowingUser = recipient }, from, count);
        return new ListResponse<UserBriefResponse>(users.Items.Select(u => new UserBriefResponse(user, u)),
            users.TotalItems, from,
            count);
    }
}
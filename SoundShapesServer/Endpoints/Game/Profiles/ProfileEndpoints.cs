using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Following;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Profiles;

public class ProfileEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{id}/~metadata:*.get", ContentType.Json)]
    public UserMetadataResponse? ViewProfile(RequestContext context, string id, GameDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);
        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page", ContentType.Json)]
    public FollowingUsersWrapper? ViewFollowingList(RequestContext context, string id, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;

        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(followedBy:follower), from, count);
        return new FollowingUsersWrapper(follower, users, totalUsers, from, count);
    }

    [GameEndpoint("~identity:{id}/~followers.page", ContentType.Json)]
    public FollowingUsersWrapper? ViewFollowersList(RequestContext context, string id, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;
        
        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(isFollowing:recipient), from, count);
        return new FollowingUsersWrapper(recipient, users, totalUsers, from, count);
    }
}
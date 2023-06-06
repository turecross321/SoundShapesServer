using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Users;

public class UserEndpoints : EndpointGroup
{
    [GameEndpoint("~index:identity.page", ContentType.Json)]
    public UsersWrapper GetUsers(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        
        UserFilters filters = UserHelper.GetUserFilters(context, database);
        UserOrderType order = UserHelper.GetUserOrderType(context);

        (GameUser[] users, int totalUsers) = database.GetUsers(order, descending, filters, from, count);
        return new UsersWrapper(user, users, totalUsers, from, count);
    }
    
    [GameEndpoint("~identity:{id}/~metadata:*.get", ContentType.Json)]
    public UserMetadataResponse? GetUser(RequestContext context, string id, GameDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);
        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page", ContentType.Json)]
    public UsersWrapper? GetFollowing(RequestContext context, string id, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(followedByUser:user), from, count);
        return new UsersWrapper(user, users, totalUsers, from, count);
    }

    [GameEndpoint("~identity:{id}/~followers.page", ContentType.Json)]
    public UsersWrapper? GetFollowers(RequestContext context, string id, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;
        
        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(isFollowingUser:recipient), from, count);
        return new UsersWrapper(recipient, users, totalUsers, from, count);
    }
}
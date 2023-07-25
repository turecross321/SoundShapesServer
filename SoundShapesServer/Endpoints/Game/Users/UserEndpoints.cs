using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Users;

public class UserEndpoints : EndpointGroup
{
    [GameEndpoint("~index:identity.page")]
    public UsersWrapper GetUsers(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        UserFilters filters = UserHelper.GetUserFilters(context, database);
        UserOrderType order = UserHelper.GetUserOrderType(context);

        (GameUser[] users, int totalUsers) = database.GetUsers(order, descending, filters, from, count);
        return new UsersWrapper(user, users, totalUsers, from, count);
    }
    
    [GameEndpoint("~identity:{id}/~metadata:*.get")]
    public UserMetadataResponse? GetUser(RequestContext context, string id, GameDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);
        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page")]
    public UsersWrapper? GetFollowing(RequestContext context, string id, GameDatabaseContext database)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);

        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(followedByUser:user), from, count);
        return new UsersWrapper(user, users, totalUsers, from, count);
    }

    [GameEndpoint("~identity:{id}/~followers.page")]
    public UsersWrapper? GetFollowers(RequestContext context, string id, GameDatabaseContext database)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);

        GameUser? recipient = database.GetUserWithId(id);
        if (recipient == null) return null;
        
        (GameUser[] users, int totalUsers) = database.GetUsers(UserOrderType.DoNotOrder, true, new UserFilters(isFollowingUser:recipient), from, count);
        return new UsersWrapper(recipient, users, totalUsers, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~metadata:{args}", ContentType.Plaintext), Authentication(false)]
    public string? GetFeaturedLevel(RequestContext context, GameDatabaseContext database, string userId, string args)
    {
        // Using args here because Bunkum doesn't support using a . as a separator
        string[] arguments = args.Split('.');
        
        string action = arguments[1];

        if (action != "get") return null;
        
        GameUser? user = database.GetUserWithId(userId);
        GameLevel? level = user?.FeaturedLevel;

        if (level == null) return null;

        return IdHelper.FormatLevelIdAndVersion(level);
    }
    
    [GameEndpoint("~identity:{userId}/~metadata:{args}", Method.Post)]
    public Response SubmitFeaturedLevel(RequestContext context, GameDatabaseContext database, GameUser user, string args, string body)
    {
        // Using args here because Bunkum doesn't support using a . as a separator
        string[] arguments = args.Split('.');
        
        string action = arguments[1];

        if (action != "put") return HttpStatusCode.NotFound;

        string levelId = IdHelper.DeFormatLevelIdAndVersion(body);
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        database.SetFeaturedLevel(user, level);
        return HttpStatusCode.OK;
    }
}
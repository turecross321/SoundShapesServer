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
    
    [GameEndpoint("~identity:{userId}/~metadata:{args}", HttpMethods.Post)]
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
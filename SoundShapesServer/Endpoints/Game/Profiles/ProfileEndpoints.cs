using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Following;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Game.Profiles;

public class ProfileEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{id}/~metadata:*.get", ContentType.Json)]
    public UserMetadataResponse? ViewProfile(RequestContext context, string id, RealmDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);

        return user == null ? null : new UserMetadataResponse(user);
    }

    [GameEndpoint("~identity:{id}/~follow:*.page", ContentType.Json)]
    public FollowingUsersWrapper? ViewFollowingList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;

        IQueryable<GameUser> users = database.GetFollowedUsers(follower);
        return new FollowingUsersWrapper(follower, users, from, count);
    }

    [GameEndpoint("~identity:{id}/~followers.page", ContentType.Json)]
    public FollowingUsersWrapper? ViewFollowersList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;
        
        IQueryable<GameUser> users = database.GetFollowers(follower);
        return new FollowingUsersWrapper(follower, users, from, count);
    }
}
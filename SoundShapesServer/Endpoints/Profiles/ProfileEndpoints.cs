using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Profiles;

public class ProfileEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~metadata:*.get", ContentType.Json)]
    public UserMetadataResponse? ViewProfile(RequestContext context, string id, RealmDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);

        if (user == null) return null;

        return UserHelper.GenerateUserMetadata(user);
    }

    [Endpoint("/otg/~identity:{id}/~follow:*.page", ContentType.Json)]
    public FollowingUserResponsesWrapper? ViewFollowingList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;

        return database.GetFollowedUsers(follower, from, count);
    }

    [Endpoint("/otg/~identity:{id}/~followers.page", ContentType.Json)]
    public FollowingUserResponsesWrapper? ViewFollowersList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;
        
        return database.GetFollowers(follower, from, count);
    }
}
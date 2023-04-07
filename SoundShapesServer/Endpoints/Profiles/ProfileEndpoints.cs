using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Profiles;

public class ProfileEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~metadata:*.get", ContentType.Json)]
    public ProfileMetadata? ViewProfile(RequestContext context, string id, RealmDatabaseContext database)
    {
        GameUser? user = database.GetUserWithId(id);

        if (user == null) return null;

        ProfileMetadata metadata = new()
        {
            displayName = user.display_name,
            follows_of_ever_count = user.followers.Count(), // Followers
            levels_by_ever_count = user.publishedLevels.Count(), // Level Count
            follows_by_ever_count = user.following.Count(), // Following
            likes_by_ever_count = user.likedLevels.Count(), // Liked And Queued Levels
        };

        return metadata;
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
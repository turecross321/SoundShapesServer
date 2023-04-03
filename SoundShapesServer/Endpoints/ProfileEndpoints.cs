using System.Data;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints;

public class ProfileEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~metadata:*.get", ContentType.Json)]
    public ProfileMetadata ViewProfile(RequestContext context, string id, RealmDatabaseContext database)
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

    private FollowingUserResponsesWrapper? GetFollowUsers(RequestContext context, RealmDatabaseContext database,
        GameUser follower, IEnumerable<GameUser> followedUsers, int from, int count)
    {
        int? nextToken;
        
        if (followedUsers.Count() <= count + from) nextToken = null;
        else nextToken = count + from;

        int? previousToken;
        if (from > 0) previousToken = from - 1;
        else previousToken = null;

        List<GameUser> userList = followedUsers.Skip(from).Take(count).ToList();

        FollowingUserResponse[] responses = new FollowingUserResponse[userList.Count()];
        
        for (int i = 0; i < userList.Count; i++)
        {
            FollowingUserResponse response = new()
            {
                id = IdFormatter.FormatFollowId(follower.id, userList[i].id),
                target = new RelationTarget()
                {
                    id = IdFormatter.FormatUserId(userList[i].id),
                    type = ResponseType.identity.ToString(),
                    displayName = userList[i].display_name
                }
            };
            
            responses[i] = response;
        }

        FollowingUserResponsesWrapper responsesWrapper = new()
        {
            items = responses,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return responsesWrapper;
    }

    [Endpoint("/otg/~identity:{id}/~follow:*.page", ContentType.Json)]
    public FollowingUserResponsesWrapper? ViewFollowingList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;
        
        IEnumerable<GameUser> followedUsers = database.GetFollowedUsers(follower);

        return GetFollowUsers(context, database, follower, followedUsers, from, count);
    }

    [Endpoint("/otg/~identity:{id}/~followers.page", ContentType.Json)]
    public FollowingUserResponsesWrapper? ViewFollowersList(RequestContext context, string id, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        GameUser? follower = database.GetUserWithId(id);
        if (follower == null) return null;
        
        IEnumerable<GameUser> followedUsers = database.GetFollowers(follower);

        return GetFollowUsers(context, database, follower, followedUsers, from, count);
    }
}
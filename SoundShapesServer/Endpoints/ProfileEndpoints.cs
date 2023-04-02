using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints;

public class ProfileEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~metadata:*.get", ContentType.Json)]
    public ProfileMetadata ViewProfile(RequestContext context, string id, RealmDatabaseContext database)
    {
        GameUser user = database.GetUserWithId(id);

        ProfileMetadata metadata = new()
        {
            displayName = user.display_name,
            follows_of_ever_count = database.GetFollowers(user).Count, // Followers
            levels_by_ever_count = database.GetLevelsPublishedByUser(user).Count, // Level Count
            follows_by_ever_count = database.GetUsersWhoUserIsFollowing(user).Count, // Following
            likes_by_ever_count = database.GetLevelsFavoritedByUser(user).Count, // Favorites And Queued
        };

        return metadata;
    }
}
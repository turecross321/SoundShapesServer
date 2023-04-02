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
        GameUser? user = database.GetUserWithId(id);

        if (user == null) return null;

        ProfileMetadata metadata = new()
        {
            displayName = user.display_name,
            follows_of_ever_count = user.followers.Count(), // Followers
            levels_by_ever_count = user.publishedLevels.Count(), // Level Count
            follows_by_ever_count = user.following.Count(), // Following
            likes_by_ever_count = user.favoriteLevels.Count(), // Favorites And Queued
        };

        return metadata;
    }
}
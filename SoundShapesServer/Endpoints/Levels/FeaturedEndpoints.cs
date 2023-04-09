using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class FeaturedEndpoints : EndpointGroup
{
    // WARNING: More than 7 community tabs will CRASH the game.
    private readonly List<CommunityTab> _communityTabs = new ()
    {
        new CommunityTab
        {
            queryType = "search",
            buttonLabel = "Newest Levels",
            query = "newest&type=level",
            panelDescription = "Check here daily for the latest cool levels! Always new stuff to check out!",
            imageUrl = "",
            panelTitle = "New Levels"
        },
        new CommunityTab
        {
            queryType = "search",
            buttonLabel = "Top Levels",
            query = "top&type=level",
            panelDescription = "Check here for the most played levels!",
            imageUrl = "",
            panelTitle = "Top Levels"
        }
    };
    
    [Endpoint("/otg/global/featured/~metadata:*.get")]
    [Endpoint("/otg/global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, RealmDatabaseContext database, string? language)
    {
        return FeaturedHelper.SerializeCommunityTabs(_communityTabs);
    }

    [Endpoint("/otg/~identity:{userId}/~metadata:featuredlevel.get", ContentType.Plaintext)]
    public string? UserFeatured(RequestContext context, RealmDatabaseContext database, string userId)
    {
        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;

        GameLevel? featuredLevel = user.featuredLevel;

        if (featuredLevel == null) return null;
        
        return IdFormatter.FormatLevelIdAndVersion(featuredLevel.id, featuredLevel.modified.ToUnixTimeMilliseconds());
    }

    [Endpoint("/otg/~identity:{userId}/~metadata:featuredLevel.put", Method.Post)]
    public Response SetUserFeaturedLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string body)
    {
        GameLevel? level = database.GetLevelWithId(IdFormatter.DeFormatLevelIdAndVersion(body));
        if (level == null) return HttpStatusCode.NotFound;

        database.SetFeaturedLevel(user, level);

        return HttpStatusCode.OK;
    }
}
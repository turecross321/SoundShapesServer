using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class FeaturedEndpoints : EndpointGroup
{
    // WARNING: More than 7 community tabs will CRASH the game.
    private readonly List<CommunityTab> _communityTabs = new ()
    {
        new CommunityTab
        {
            QueryType = "search",
            ButtonLabel = "Newest Levels",
            Query = "newest&type=level",
            PanelDescription = "Check here daily for the latest cool levels! Always new stuff to check out!",
            ImageUrl = "",
            PanelTitle = "New Levels"
        },
        new CommunityTab
        {
            QueryType = "search",
            ButtonLabel = "Top Levels",
            Query = "top&type=level",
            PanelDescription = "Check here for the most played levels!",
            ImageUrl = "",
            PanelTitle = "Top Levels"
        }
    };
    
    [GameEndpoint("global/featured/~metadata:*.get")]
    [GameEndpoint("global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, RealmDatabaseContext database, string? language)
    {
        return FeaturedHelper.SerializeCommunityTabs(_communityTabs);
    }

    [GameEndpoint("~identity:{userId}/~metadata:featuredlevel.get", ContentType.Plaintext)]
    public string? UserFeatured(RequestContext context, RealmDatabaseContext database, string userId)
    {
        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;

        GameLevel? featuredLevel = user.FeaturedLevel;

        if (featuredLevel == null) return null;
        
        return IdFormatter.FormatLevelIdAndVersion(featuredLevel.Id, featuredLevel.ModificationDate.ToUnixTimeMilliseconds());
    }

    [GameEndpoint("~identity:{userId}/~metadata:featuredLevel.put", Method.Post)]
    public Response SetUserFeaturedLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string body)
    {
        GameLevel? level = database.GetLevelWithId(IdFormatter.DeFormatLevelIdAndVersion(body));
        if (level == null) return HttpStatusCode.NotFound;

        database.SetUserFeaturedLevel(user, level);

        return HttpStatusCode.OK;
    }
}
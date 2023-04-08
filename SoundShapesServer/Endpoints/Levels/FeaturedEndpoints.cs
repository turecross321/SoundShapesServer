using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NotEnoughLogs.Definitions;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Responses.RecentActivity;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Levels;

public class FeaturedEndpoints : EndpointGroup
{
    // TODO: MAKE THIS MODULAR AND ADD SUPPORT FOR DIFFERENT LANGUAGES
    [Endpoint("/otg/global/featured/~metadata:*.get")]
    [Endpoint("/otg/global/featured/{language}/~metadata:*.get", ContentType.Json)]
    public FeaturedResponse GlobalFeatured(RequestContext context, string language)
    {
        return new FeaturedResponse()
        {
            queryType = "search",
            buttonLabel = "New Releases",
            query = "newest\u0026type\u003dlevel",
            panelDescription = "Check here daily for the latest cool levels! Always new stuff to check out!",
            imageUrl = "", // TODO: implement this
            panelTitle = "New Releases",
        };
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
    public Response SetFeaturedLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string body)
    {
        GameLevel? level = database.GetLevelWithId(IdFormatter.DeFormatLevelIdAndVersion(body));
        if (level == null) return HttpStatusCode.NotFound;

        database.SetFeaturedLevel(user, level);

        return HttpStatusCode.OK;
    }
}
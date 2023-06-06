using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Endpoints.Game.CommunityTabs;

public class CommunityTabsEndpoint : EndpointGroup
{
    [GameEndpoint("global/featured/~metadata:*.get", ContentType.Plaintext)]
    [GameEndpoint("global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, GameDatabaseContext database, string? language)
    {
        return CommunityTabHelper.SerializeCommunityTabs(database.GetCommunityTabs());
    }
}
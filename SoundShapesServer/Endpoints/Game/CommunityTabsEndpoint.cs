using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Listener.Protocol;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Endpoints.Game;

public class CommunityTabsEndpoint : EndpointGroup
{
    [GameEndpoint("global/featured/~metadata:*.get", ContentType.Plaintext)]
    [GameEndpoint("global/featured/{language}/~metadata:*.get", ContentType.Plaintext)]
    public string GlobalFeatured(RequestContext context, GameDatabaseContext database, string? language)
    {
        return CommunityTabHelper.SerializeCommunityTabs(database.GetCommunityTabs());
    }
}
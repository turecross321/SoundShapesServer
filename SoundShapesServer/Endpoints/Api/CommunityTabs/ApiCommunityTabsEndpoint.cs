using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Responses.Api.CommunityTabs;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.CommunityTabs;

public class ApiCommunityTabsEndpoint : EndpointGroup
{
    [ApiEndpoint("communityTabs"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists community tabs.")]
    public ApiCommunityTabsWrapper GetCommunityTabs(RequestContext context, GameDatabaseContext database)
    {
        CommunityTab[] communityTabs = database.GetCommunityTabs();
        return new ApiCommunityTabsWrapper(communityTabs);
    }

    [ApiEndpoint("communityTabs/id/{id}"), Authentication(false)]
    [DocSummary("Retrieves community tab with specified ID.")]
    public ApiCommunityTabResponse? GetCommunityTab(RequestContext context, GameDatabaseContext database, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        return communityTab != null ? new ApiCommunityTabResponse(communityTab) : null;
    }
}
using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.CommunityTabs;

public class ApiCommunityTabsEndpoint : EndpointGroup
{
    [ApiEndpoint("communityTabs")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists community tabs.")]
    public ApiListResponse<ApiCommunityTabResponse> GetCommunityTabs(RequestContext context,
        GameDatabaseContext database)
    {
        CommunityTab[] communityTabs = database.GetCommunityTabs();
        return new ApiListResponse<ApiCommunityTabResponse>(ApiCommunityTabResponse.FromOldList(communityTabs));
    }

    [ApiEndpoint("communityTabs/id/{id}")]
    [Authentication(false)]
    [DocSummary("Retrieves community tab with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.CommunityTabNotFoundWhen)]
    [DocRouteParam("id", "Community tab ID.")]
    public ApiResponse<ApiCommunityTabResponse> GetCommunityTab(RequestContext context, GameDatabaseContext database,
        string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        if (communityTab == null)
            return ApiNotFoundError.CommunityTabNotFound;

        return ApiCommunityTabResponse.FromOld(communityTab);
    }
}
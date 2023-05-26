using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.CommunityTabs;

public class ApiCommunityTabsWrapper
{
    public ApiCommunityTabsWrapper(CommunityTab[] communityTabs)
    {
        CommunityTabs = communityTabs.Select(t=>new ApiCommunityTabResponse(t)).ToArray();
        Count = communityTabs.Length;
    }

    public ApiCommunityTabResponse[] CommunityTabs { get; set; }
    public int Count { get; set; }
}
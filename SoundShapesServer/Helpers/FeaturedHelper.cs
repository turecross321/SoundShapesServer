using Newtonsoft.Json.Linq;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class FeaturedHelper
{
    public static string SerializeCommunityTabs(List<CommunityTab> communityTabs)
    {
        JObject jObject = new();

        for (int i = 0; i < communityTabs.Count; i++)
        {
            CommunityTab communityTab = communityTabs[i];

            jObject.Add($"{i:00}_queryType", "search");
            jObject.Add($"{i:00}_buttonLabel", communityTab.ButtonLabel);
            jObject.Add($"{i:00}_query", communityTab.Query + "&type=level");
            jObject.Add($"{i:00}_panelDescription", communityTab.PanelDescription);
            jObject.Add($"{i:00}_imageUrl", communityTab.ImageUrl);
            jObject.Add($"{i:00}_panelTitle", communityTab.PanelTitle);
        }

        return jObject.ToString();
    }
}
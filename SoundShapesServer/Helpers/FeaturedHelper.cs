using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public class FeaturedHelper
{
    public static string SerializeCommunityTabs(List<CommunityTab> communityTabs)
    {
        JObject jObject = new JObject();

        for (int i = 0; i < communityTabs.Count; i++)
        {
            CommunityTab communityTab = communityTabs[i];

            jObject.Add($"{i:00}_queryType", communityTab.queryType);
            jObject.Add($"{i:00}_buttonLabel", communityTab.buttonLabel);
            jObject.Add($"{i:00}_query", communityTab.query);
            jObject.Add($"{i:00}_panelDescription", communityTab.panelDescription);
            jObject.Add($"{i:00}_imageUrl", communityTab.imageUrl);
            jObject.Add($"{i:00}_panelTitle", communityTab.panelTitle);
        }

        return jObject.ToString();
    }
}
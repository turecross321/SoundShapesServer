using Newtonsoft.Json.Linq;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class CommunityTabHelper
{
    public static string SerializeCommunityTabs(CommunityTab[] communityTabs)
    {
        JObject jObject = new();

        for (int i = 0; i < communityTabs.Length; i++)
        {
            CommunityTab communityTab = communityTabs[i];

            jObject.Add($"{i:00}_panelTitle", communityTab.Title);
            jObject.Add($"{i:00}_panelDescription", communityTab.Description);
            jObject.Add($"{i:00}_queryType", "search");
            jObject.Add($"{i:00}_buttonLabel", communityTab.ButtonLabel);
            // We're adding a & before the query here so that we can type whatever query we want and not
            // have to adhere to the search query
            jObject.Add($"{i:00}_query", "&" + communityTab.Query 
                                             + "&type=" + ContentHelper.GetContentTypeString(communityTab.ContentType));
            
            if (communityTab.ThumbnailFilePath != null)
                jObject.Add($"{i:00}_imageUrl", ResourceHelper.GetCommunityTabThumbnailUrl(communityTab.Id));
        }

        return jObject.ToString();
    }
}
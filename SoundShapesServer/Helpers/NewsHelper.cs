using Newtonsoft.Json.Linq;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.RecentActivity;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public class NewsHelper
{
    public static NewsResponse NewsEntryToNewsResponse(NewsEntry entry)
    {
        return new NewsResponse()
        {
            title = entry.title,
            text = entry.text,
            fullText = entry.fullText,
            url = entry.url
        };
    }
}
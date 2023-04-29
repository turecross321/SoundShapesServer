using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class NewsHelper
{
    public static NewsResponse NewsEntryToNewsResponse(NewsEntry? entry)
    {
        if (entry == null) return new NewsResponse();
        
        return new NewsResponse()
        {
            Title = entry.Title,
            Summary = entry.Summary,
            FullText = entry.FullText,
            Url = entry.Url,
        };
    }
}
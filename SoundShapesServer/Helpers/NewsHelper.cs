using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class NewsHelper
{
    public static NewsResponse NewsEntryToNewsResponse(NewsEntry entry)
    {
        return new NewsResponse()
        {
            Title = entry.Title,
            Summary = entry.Summary,
            FullText = entry.FullText,
            Url = entry.Url,
        };
    }
}
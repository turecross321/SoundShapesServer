using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class NewsHelper
{
    public static IQueryable<NewsEntry> OrderNews(IQueryable<NewsEntry> entries, NewsOrderType orderType)
    {
        return orderType switch
        {
            NewsOrderType.Date => entries.OrderBy(e=>e.Date),
            NewsOrderType.Length => entries.OrderBy(e=>e.FullText.Length),
            _ => OrderNews(entries, NewsOrderType.Date)
        };
    }

    public static IQueryable<NewsEntry> FilterNews(IQueryable<NewsEntry> entries, string language)
    {
        return entries.Where(e => e.Language == language);
    }
}
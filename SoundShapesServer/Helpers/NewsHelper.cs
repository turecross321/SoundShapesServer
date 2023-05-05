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

    public static IQueryable<NewsEntry> FilterNews(IQueryable<NewsEntry> entries, string? language, string? byUser)
    {
        IQueryable<NewsEntry> response = entries;

        if (language != null)
        {
            response = response.Where(e => e.Language == language);
        }

        if (byUser != null)
        {
            response = response.Where(e => e.Author != null && e.Author.Id == byUser);
        }

        return response;
    }
}
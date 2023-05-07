using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Helpers;

public static class NewsHelper
{
    public static IQueryable<NewsEntry> OrderNews(IQueryable<NewsEntry> entries, NewsOrderType orderType, bool descending)
    {
        IQueryable<NewsEntry> response = entries;
        
        response = orderType switch
        {
            NewsOrderType.Date => response.OrderBy(e=>e.CreationDate),
            NewsOrderType.Length => response.OrderBy(e=>e.FullText.Length),
            _ => OrderNews(response, NewsOrderType.Date, descending)
        };

        if (descending) response = response.AsEnumerable().Reverse().AsQueryable();

        return response;
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
            response = response.Where(e => e.Author.Id == byUser);
        }

        return response;
    }
}
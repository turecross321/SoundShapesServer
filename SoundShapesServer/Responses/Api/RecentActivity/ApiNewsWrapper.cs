using SoundShapesServer.Helpers;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiNewsWrapper
{
    public ApiNewsWrapper(IQueryable<NewsEntry> entries, int from, int count, NewsOrderType orderType, bool descending)
    {
        IQueryable<NewsEntry> orderedEntries = NewsHelper.OrderNews(entries, orderType, descending);

        NewsEntry[] paginatedNewsEntries = PaginationHelper.PaginateNews(orderedEntries, from, count);
        
        Entries = paginatedNewsEntries.Select(l=> new ApiNewsResponse(l)).ToArray();
        Count = orderedEntries.Count();
    }

    public ApiNewsResponse[] Entries { get; set; }
    public int Count { get; set; }
}
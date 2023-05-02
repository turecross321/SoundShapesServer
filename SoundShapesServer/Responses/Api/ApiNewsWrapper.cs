using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiNewsWrapper
{
    public ApiNewsWrapper(IQueryable<NewsEntry> entries, int from, int count, NewsOrderType orderType, bool descending)
    {
        IQueryable<NewsEntry> orderedEntries = NewsHelper.OrderNews(entries, orderType);
        IQueryable<NewsEntry> fullyOrderedEntries = descending ? orderedEntries.AsEnumerable().Reverse().AsQueryable() : orderedEntries;

        NewsEntry[] paginatedNewsEntries = PaginationHelper.PaginateNews(fullyOrderedEntries, from, count);
        
        Entries = paginatedNewsEntries.Select(l=> new ApiNewsResponse(l)).ToArray();
        Count = fullyOrderedEntries.Count();
    }

    public ApiNewsResponse[] Entries { get; set; }
    public int Count { get; set; }
}
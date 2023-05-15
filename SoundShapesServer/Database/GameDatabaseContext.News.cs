using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public (NewsEntry[], int) GetNews(NewsOrderType order, bool descending, NewsFilters filters, int from, int count)
    {
        IQueryable<NewsEntry> orderedNews = order switch
        {
            NewsOrderType.CreationDate => NewsOrderedByCreationDate(descending),
            NewsOrderType.ModificationDate => NewsOrderedByModificationDate(descending),
            NewsOrderType.CharacterCount => NewsOrderedByLength(descending),
            _ => NewsOrderedByCreationDate(descending)
        };

        IQueryable<NewsEntry> filteredNews = FilterNews(orderedNews, filters);
        NewsEntry[] paginatedNews = PaginationHelper.PaginateNews(filteredNews, from, count);

        return (paginatedNews, filteredNews.Count());
    }
    
    private IQueryable<NewsEntry> FilterNews(IQueryable<NewsEntry> entries, NewsFilters filters)
    {
        IQueryable<NewsEntry> response = entries;

        if (filters.Language != null)
        {
            response = response.Where(e => e.Language == filters.Language);
        }

        if (filters.Authors != null)
        {
            IEnumerable<NewsEntry> tempResponse = new List<NewsEntry>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (GameUser author in filters.Authors)
            {
                tempResponse = tempResponse.Concat(response.Where(e=> e.Author == author));
            }

            response = tempResponse.AsQueryable();
        }

        return response;
    }

    private IQueryable<NewsEntry> NewsOrderedByCreationDate(bool descending)
    {
        if (descending) return _realm.All<NewsEntry>().OrderByDescending(e => e.CreationDate);
        return _realm.All<NewsEntry>().OrderBy(e => e.CreationDate);
    }
    
    private IQueryable<NewsEntry> NewsOrderedByModificationDate(bool descending)
    {
        if (descending) return _realm.All<NewsEntry>().OrderByDescending(e => e.ModificationDate);
        return _realm.All<NewsEntry>().OrderBy(e => e.ModificationDate);
    }
    
    private IQueryable<NewsEntry> NewsOrderedByLength(bool descending)
    {
        if (descending)
            return _realm.All<NewsEntry>()
                .OrderByDescending(e => e.CharacterCount);
        return _realm.All<NewsEntry>()
            .OrderBy(e => e.CharacterCount);
    }

    public NewsEntry? GetNewsEntryWithId(string id)
    {
        return _realm.All<NewsEntry>().FirstOrDefault(e => e.Id == id);
    }

    public NewsEntry CreateNewsEntry(ApiCreateNewsEntryRequest request, GameUser user)
    {
        NewsEntry entry = new (user, request);
        
        _realm.Write(() =>
        {
            _realm.Add(entry);
        });

        return entry;
    }

    public NewsEntry EditNewsEntry(NewsEntry entry, ApiCreateNewsEntryRequest request)
    {
        _realm.Write(() =>
        {
            entry.Language = request.Language ?? "global";
            entry.Title = request.Title ?? "";
            entry.Summary = request.Summary ?? "";
            entry.FullText = request.FullText ?? "";
            entry.Url = request.Url ?? "";
            entry.ModificationDate = DateTimeOffset.UtcNow;
            entry.CharacterCount = entry.FullText.Length;
        });

        return entry;
    }

    public void RemoveNewsEntry(IDataStore dataStore, NewsEntry entry)
    {
        dataStore.RemoveFromStore(ResourceHelper.GetNewsResourceKey(entry.Id));
        
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}
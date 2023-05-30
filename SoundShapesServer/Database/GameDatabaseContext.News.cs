using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public NewsEntry CreateNewsEntry(ApiCreateNewsEntryRequest request, GameUser user)
    {
        NewsEntry entry = new (user, request);
        
        _realm.Write(() =>
        {
            _realm.Add(entry);
        });

        return entry;
    }

    public NewsEntry EditNewsEntry(NewsEntry entry, ApiCreateNewsEntryRequest request, GameUser user)
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
            entry.Author = user;
        });

        return entry;
    }

    public Response UploadNewsResource(IDataStore dataStore, NewsEntry newsEntry, byte[] file)
    {
        if (!IsByteArrayPng(file)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        string key = GetNewsResourceKey(newsEntry.Id);
        dataStore.WriteToStore(key, file);

        _realm.Write(() =>
        {
            newsEntry.ThumbnailFilePath = key;
        });

        return HttpStatusCode.Created;
    }
    
    public void RemoveNewsEntry(IDataStore dataStore, NewsEntry entry)
    {
        if (entry.ThumbnailFilePath != null) dataStore.RemoveFromStore(entry.ThumbnailFilePath);

        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
    
    public NewsEntry? GetNewsEntryWithId(string id)
    {
        return _realm.All<NewsEntry>().FirstOrDefault(e => e.Id == id);
    }
    
    public (NewsEntry[], int) GetNews(NewsOrderType order, bool descending, NewsFilters filters, int from, int count)
    {
        IQueryable<NewsEntry> entries = _realm.All<NewsEntry>();

        IQueryable<NewsEntry> filteredEntries = FilterNews(entries, filters);
        IQueryable<NewsEntry> orderedEntries = OrderNews(filteredEntries, order, descending);
        
        NewsEntry[] paginatedNews = PaginationHelper.PaginateNews(orderedEntries, from, count);

        return (paginatedNews, filteredEntries.Count());
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

    #region News Ordering
    
    private IQueryable<NewsEntry> OrderNews(IQueryable<NewsEntry> entries, NewsOrderType order, bool descending)
    {
        return order switch
        {
            NewsOrderType.CreationDate => OrderNewsByCreationDate(entries, descending),
            NewsOrderType.ModificationDate => OrderNewsByModificationDate(entries, descending),
            NewsOrderType.CharacterCount => OrderNewsByLength(entries, descending),
            _ => OrderNewsByCreationDate(entries, descending)
        };
    }

    private static IQueryable<NewsEntry> OrderNewsByCreationDate(IQueryable<NewsEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.CreationDate);
        return entries.OrderBy(e => e.CreationDate);
    }
    
    private static IQueryable<NewsEntry> OrderNewsByModificationDate(IQueryable<NewsEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.ModificationDate);
        return entries.OrderBy(e => e.ModificationDate);
    }
    
    private static IQueryable<NewsEntry> OrderNewsByLength(IQueryable<NewsEntry> entries, bool descending)
    {
        if (descending)
            return entries
                .OrderByDescending(e => e.CharacterCount);
        return entries
            .OrderBy(e => e.CharacterCount);
    }
    
    #endregion
}
using Bunkum.Core.Storage;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
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

    public ApiOkResponse UploadNewsResource(IDataStore dataStore, NewsEntry newsEntry, byte[] file)
    {
        if (!IsByteArrayPng(file)) return ApiBadRequestError.FileIsNotPng;

        string key = GetNewsResourceKey(newsEntry.Id);
        dataStore.WriteToStore(key, file);

        _realm.Write(() =>
        {
            newsEntry.ThumbnailFilePath = key;
        });

        return new ApiOkResponse();
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
    
    public (NewsEntry[], int) GetPaginatedNews(NewsOrderType order, bool descending, NewsFilters filters, int from, int count)
    {
        IQueryable<NewsEntry> entries = _realm.All<NewsEntry>().FilterNews(filters).OrderNews(order, descending);
        return (entries.Paginate(from, count), entries.Count());
    }
}
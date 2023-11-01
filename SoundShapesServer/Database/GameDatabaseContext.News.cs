using Bunkum.Core.Storage;
using MongoDB.Bson;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public NewsEntry CreateNewsEntry(ApiCreateNewsEntryRequest request, GameUser user)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        NewsEntry entry = new NewsEntry
        {
            CreationDate = now,
            ModificationDate = now,
            Author = user,
            Language = request.Language,
            Title = request.Title,
            Summary = request.Summary,
            FullText = request.FullText,
            Url = string.IsNullOrEmpty(request.Url) ? "0.0.0.0" : request.Url, // A url crashes the Vita version
            CharacterCount = request.FullText.Length,
        };
        
        _realm.Write(() =>
        {
            _realm.Add(entry);
        });

        CreateEvent(user, EventType.NewsCreation, PlatformType.Unknown, EventDataType.NewsEntry, entry.Id.ToString()!);        
        
        return entry;
    }

    public NewsEntry EditNewsEntry(NewsEntry entry, ApiCreateNewsEntryRequest request, GameUser user)
    {
        _realm.Write(() =>
        {
            entry.Language = request.Language;
            entry.Title = request.Title;
            entry.Summary = request.Summary;
            entry.FullText = request.FullText;
            entry.Url = request.Url;
            entry.ModificationDate = DateTimeOffset.UtcNow;
            entry.CharacterCount = entry.FullText.Length;
            entry.Author = user;
        });

        return entry;
    }

    public ApiOkResponse UploadNewsResource(IDataStore dataStore, NewsEntry newsEntry, byte[] file)
    {
        if (!file.IsPng()) return
            ApiBadRequestError.FileIsNotPng;

        string key = GetNewsResourceKey(newsEntry.Id.ToString()!);
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
        if (!ObjectId.TryParse(id, out ObjectId objectId)) 
            return null;
        
        return _realm.All<NewsEntry>().FirstOrDefault(e => e.Id == objectId);
    }
    
    public PaginatedList<NewsEntry> GetPaginatedNews(NewsOrderType order, bool descending, NewsFilters filters, int from, int count)
    {
        return new PaginatedList<NewsEntry>(_realm.All<NewsEntry>().FilterNews(filters).OrderNews(order, descending), from, count);
    }
}
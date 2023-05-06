using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<NewsEntry> GetNews()
    {
        return _realm.All<NewsEntry>();
    }

    public NewsEntry? GetNewsEntryWithId(string id)
    {
        return _realm.All<NewsEntry>().FirstOrDefault(e => e.Id == id);
    }

    public NewsEntry CreateNewsEntry(ApiCreateNewsEntryRequest request, GameUser user)
    {
        NewsEntry entry = new ()
        {
            Id = GenerateGuid(),
            Date = DateTimeOffset.UtcNow,
            Author = user,
            Language = request.Language,
            Title = request.Title,
            Summary = request.Summary,
            FullText = request.FullText,
            Url = request.Url
        };
        
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
            entry.Language = request.Language;
            entry.Title = request.Title;
            entry.Summary = request.Summary;
            entry.FullText = request.FullText;
            entry.Url = request.Url;
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
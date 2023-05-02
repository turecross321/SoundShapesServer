using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

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

    public NewsEntry CreateNewsEntry(ApiCreateNewsEntryRequest request)
    {
        NewsEntry entry = new ()
        {
            Id = GenerateGuid(),
            Date = DateTimeOffset.UtcNow,
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

    public void RemoveNewsEntry(NewsEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}
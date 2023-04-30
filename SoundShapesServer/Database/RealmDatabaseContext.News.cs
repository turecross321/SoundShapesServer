using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public NewsEntry? GetNews(string language)
    {
        NewsEntry? translatedNews = _realm.All<NewsEntry>().FirstOrDefault(n => n.Language == language);
        return translatedNews;
    }

    public void CreateNewsEntry(NewsEntry entry)
    {
        _realm.Write(() =>
        {
            // Remove previous entries with the same language
            _realm.RemoveRange(_realm.All<NewsEntry>().Where(e=>e.Language == entry.Language));
            
            _realm.Add(entry);
        });
    }

    public void RemoveNewsEntry(NewsEntry entry)
    {
        _realm.Write(() =>
        {
            _realm.Remove(entry);
        });
    }
}
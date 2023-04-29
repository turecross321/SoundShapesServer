using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public NewsEntry? GetNews(string language)
    {
        NewsEntry? translatedNews = this._realm.All<NewsEntry>().FirstOrDefault(n => n.Language == language);
        return translatedNews;
    }

    public void CreateNewsEntry(string language, string title, string summary, string fullText, string url)
    {

        NewsEntry entry = new NewsEntry()
        {
            Language = language,
            Title = title,
            Summary = summary,
            FullText = fullText,
            Url = url
        };
        
        this._realm.Write(() =>
        {
            // Remove previous entries with the same language
            this._realm.RemoveRange<NewsEntry>(this._realm.All<NewsEntry>().Where(e=>e.Language == language));
            
            this._realm.Add(entry);
        });
    }
}
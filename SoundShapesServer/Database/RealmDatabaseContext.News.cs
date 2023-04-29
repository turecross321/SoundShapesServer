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

    public void CreateNewsEntry(ApiNewsEntryRequest request)
    {
        NewsEntry entry = new NewsEntry()
        {
            Language = request.Language,
            Title = request.Title,
            Summary = request.Summary,
            FullText = request.FullText,
            Url = request.Url
        };
        
        this._realm.Write(() =>
        {
            this._realm.Add(entry);
        });
    }
}
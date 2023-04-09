using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public NewsEntry GetGlobalNews()
    {
        NewsEntry? news = this._realm.All<NewsEntry>().FirstOrDefault(n => n.Language== "global");
        
        if (news != null) return news;
        else return CreateNewsEntry(new NewsEntry()
        {
            Language = "global",
            Summary = "Welcome back to Sound Shapes!",
            Title = "News",
            FullText = "",
            Url = "https://example.com/"
        });
    }

    public NewsEntry GetTranslatedNews(string language)
    {
        NewsEntry? translatedNews = this._realm.All<NewsEntry>().FirstOrDefault(n => n.Language == language);

        if (translatedNews == null) return GetGlobalNews();

        return translatedNews;
    }

    public NewsEntry CreateNewsEntry(NewsEntry newsEntry)
    {
        this._realm.Write(() =>
        {
            this._realm.Add(newsEntry);
        });

        return newsEntry;
    }
}
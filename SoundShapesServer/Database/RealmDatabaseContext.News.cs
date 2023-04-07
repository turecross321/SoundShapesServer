using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public NewsEntry GetGlobalNews()
    {
        NewsEntry? news = this._realm.All<NewsEntry>().Where(n=>n.language== "global").FirstOrDefault();
        
        if (news != null) return news;
        else return CreateNewsEntry(new NewsEntry()
        {
            language = "global",
            text = "Welcome back to Sound Shapes!",
            title = "News",
            fullText = "",
            url = "https://example.com/"
        });
    }

    public NewsEntry GetTranslatedNews(string language)
    {
        NewsEntry? globalNews = GetGlobalNews();
        NewsEntry? translatedNews = this._realm.All<NewsEntry>().FirstOrDefault(n => n.language == language);

        NewsEntry newsToReturn = new NewsEntry();

        if (translatedNews != null)
        {
            newsToReturn.text = translatedNews.text;
            newsToReturn.title = translatedNews.title;
            newsToReturn.fullText = translatedNews.fullText;
            newsToReturn.url = translatedNews.url;   
        }

        if (newsToReturn.text == null) newsToReturn.text = globalNews.text;
        if (newsToReturn.title == null) newsToReturn.title = globalNews.title;
        if (newsToReturn.fullText == null) newsToReturn.fullText = globalNews.fullText;
        if (newsToReturn.url == null) newsToReturn.url = globalNews.url;

        return newsToReturn;
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
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [GameEndpoint("global/news/~metadata:*.get", ContentType.Json)]
    public NewsResponse GlobalNews(RequestContext context, RealmDatabaseContext database)
    {
        IQueryable<NewsEntry> entries = database.GetNews();
        IQueryable<NewsEntry> filteredNews = NewsHelper.FilterNews(entries, "global", null);
        NewsEntry? entry = filteredNews.LastOrDefault();

        return entry == null ? new NewsResponse() : new NewsResponse(entry);
    }

    
    [GameEndpoint("global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse? TranslatedNews(RequestContext context, GameSession session, RealmDatabaseContext database, string language)
    {
        IQueryable<NewsEntry> entries = database.GetNews();
        IQueryable<NewsEntry> filteredNews = NewsHelper.FilterNews(entries, language, null);
        NewsEntry? entry = filteredNews.LastOrDefault();

        return entry == null ? null : new NewsResponse(entry);
    }
}
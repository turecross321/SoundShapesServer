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
    public NewsResponse GlobalNews(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        IQueryable<NewsEntry> entries = database.GetNews();
        IQueryable<NewsEntry> filteredNews = NewsHelper.FilterNews(entries, "global", null);
        NewsEntry? entry = filteredNews.LastOrDefault();

        // News images make the vita version crash, so this is a workaround that only lets non-vita view them
        bool isUserOnVita = session.PlatformType == (int)PlatformType.PsVita;
        return entry == null ? new NewsResponse() : new NewsResponse(entry, !isUserOnVita);
    }

    
    [GameEndpoint("global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse? TranslatedNews(RequestContext context, GameSession session, GameDatabaseContext database, string language)
    {
        IQueryable<NewsEntry> entries = database.GetNews();
        IQueryable<NewsEntry> filteredNews = NewsHelper.FilterNews(entries, language, null);
        NewsEntry? entry = filteredNews.LastOrDefault();

        // News images make the vita version crash, so this is a workaround that only lets non-vita view them
        bool isUserOnVita = session.PlatformType == (int)PlatformType.PsVita;
        return entry == null ? null : new NewsResponse(entry, !isUserOnVita);
    }
}
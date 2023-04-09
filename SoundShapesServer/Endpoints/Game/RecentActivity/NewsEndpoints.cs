using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.NewsHelper;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [GameEndpoint("global/news/~metadata:*.get", ContentType.Json)]
    public NewsResponse GlobalNews(RequestContext context, RealmDatabaseContext database)
    {
        NewsEntry news = database.GetGlobalNews();
        return NewsEntryToNewsResponse(news);
    }

    
    [GameEndpoint("global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse TranslatedNews(RequestContext context, Session session, RealmDatabaseContext database, string language)
    {
        NewsEntry news = database.GetTranslatedNews(language);
        return NewsEntryToNewsResponse(news);
    }
}
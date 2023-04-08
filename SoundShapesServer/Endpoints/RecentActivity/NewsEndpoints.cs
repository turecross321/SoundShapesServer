using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints.Albums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.RecentActivity;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.NewsHelper;

namespace SoundShapesServer.Endpoints.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [Endpoint("/otg/global/news/~metadata:*.get", ContentType.Json)]
    public NewsResponse GlobalNews(RequestContext context, RealmDatabaseContext database)
    {
        NewsEntry news = database.GetGlobalNews();
        return NewsEntryToNewsResponse(news);
    }

    
    [Endpoint("/otg/global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse TranslatedNews(RequestContext context, GameSession session, RealmDatabaseContext database, string language)
    {
        NewsEntry news = database.GetTranslatedNews(language);
        return NewsEntryToNewsResponse(news);
    }
}
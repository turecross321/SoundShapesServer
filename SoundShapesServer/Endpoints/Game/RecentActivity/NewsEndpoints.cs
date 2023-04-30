using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [GameEndpoint("global/news/~metadata:*.get", ContentType.Json)]
    public NewsResponse GlobalNews(RequestContext context, RealmDatabaseContext database)
    {
        NewsEntry news = database.GetNews("global") ?? new NewsEntry();
        return new NewsResponse(news);
    }

    
    [GameEndpoint("global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse? TranslatedNews(RequestContext context, GameSession session, RealmDatabaseContext database, string language)
    {
        NewsEntry? news = database.GetNews(language);

        return news == null ? null : new NewsResponse(news);
    }
}
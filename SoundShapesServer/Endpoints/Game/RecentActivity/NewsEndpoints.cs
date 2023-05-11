using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Endpoints.Game.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [GameEndpoint("global/news/~metadata:*.get", ContentType.Json)]
    [GameEndpoint("global/news/{language}/~metadata:*.get", ContentType.Json)]
    public NewsResponse GetNews(RequestContext context, GameDatabaseContext database, string? language, GameSession session)
    {
        NewsFilters filters = new (language);

        // Game only gets the last news entry
        (NewsEntry[] entries, int _) = database.GetNews(NewsOrderType.CreationDate, true, filters, 0, 1);
        NewsEntry? entry = entries.LastOrDefault();

        // News images make the vita version crash, so this is a workaround that only lets non-vita view them
        bool isUserOnVita = session.PlatformType == (int)PlatformType.PsVita;
        return entry == null ? new NewsResponse() : new NewsResponse(entry, !isUserOnVita);
    }
}
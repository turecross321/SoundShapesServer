using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Endpoints.Game;

public class NewsEndpoints : EndpointGroup
{
    [GameEndpoint("global/news/~metadata:*.get")]
    [GameEndpoint("global/news/{language}/~metadata:*.get")]
    public NewsResponse GetNews(RequestContext context, GameDatabaseContext database, string? language, GameToken token)
    {
        NewsFilters filters = new (language);

        // Game only gets the last news entry
        (NewsEntry[] entries, int _) = database.GetPaginatedNews(NewsOrderType.CreationDate, true, filters, 0, 1);
        NewsEntry? entry = entries.LastOrDefault();

        // News images make the vita version crash, so this is a workaround that only lets non-vita view them
        bool isUserOnVita = token.PlatformType == PlatformType.PsVita;
        return new NewsResponse(entry, !isUserOnVita);
    }
}
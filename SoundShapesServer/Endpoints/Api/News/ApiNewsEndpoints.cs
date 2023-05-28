using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.RecentActivity;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news/id/{id}")]
    [Authentication(false)]
    public ApiNewsResponse? NewsEntryWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        return newsEntry != null ? new ApiNewsResponse(newsEntry) : null;
    }
    
    [ApiEndpoint("news")]
    [Authentication(false)]
    public ApiNewsWrapper News(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? language = context.QueryString["language"];
        string? authorIds = context.QueryString["authors"];
        List<GameUser>? authors = null;

        if (authorIds != null)
        {
            authors = new List<GameUser>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (string authorId in authorIds.Split(","))
            {
                GameUser? author = database.GetUserWithId(authorId);
                if (author != null) authors.Add(author);
            }
        }

        NewsFilters filters = new (language, authors?.ToArray());

        NewsOrderType order = orderString switch
        {
            "creationDate" => NewsOrderType.CreationDate,
            "modificationDate" => NewsOrderType.ModificationDate,
            "characterCount" => NewsOrderType.CharacterCount,
            _ => NewsOrderType.CreationDate
        };

        (NewsEntry[] entries, int totalEntries) = database.GetNews(order, descending, filters, from, count);
        return new ApiNewsWrapper(entries, totalEntries);
    }
}
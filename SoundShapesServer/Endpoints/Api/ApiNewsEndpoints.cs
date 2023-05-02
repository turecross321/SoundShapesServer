using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news")]
    [Authentication(false)]
    public ApiNewsWrapper News(RequestContext context, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string language = context.QueryString["language"] ?? "global";
        
        IQueryable<NewsEntry> entries = database.GetNews();
        IQueryable<NewsEntry> filteredEntries = NewsHelper.FilterNews(entries, language);
        
        NewsOrderType order = orderString switch
        {
            "date" => NewsOrderType.Date,
            "length" => NewsOrderType.Length,
            _ => NewsOrderType.Date
        };

        return new ApiNewsWrapper(filteredEntries, from, count, order, descending);
    }
}
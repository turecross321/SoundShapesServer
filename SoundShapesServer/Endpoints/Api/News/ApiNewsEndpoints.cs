using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news/id/{id}"), Authentication(false)]
    [DocSummary("Retrieves news entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NewsEntryNotFoundWhen)]
    public ApiResponse<ApiNewsEntryResponse> NewsEntryWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null)
            return ApiNotFoundError.NewsEntryNotFound;
        
        return new ApiNewsEntryResponse(newsEntry);
    }
    
    [ApiEndpoint("news"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists news.")]
    public ApiListResponse<ApiNewsEntryResponse> News(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();
        
        string? orderString = context.QueryString["orderBy"];

        string? language = context.QueryString["language"];
        List<GameUser>? authors = context.QueryString["authors"].ToUsers(database);

        NewsFilters filters = new NewsFilters
        {
            Language = language,
            Authors = authors?.ToArray()
        };

        NewsOrderType order = orderString switch
        {
            "creationDate" => NewsOrderType.CreationDate,
            "modificationDate" => NewsOrderType.ModificationDate,
            "characterCount" => NewsOrderType.CharacterCount,
            _ => NewsOrderType.CreationDate
        };

        (NewsEntry[] entries, int totalEntries) = database.GetPaginatedNews(order, descending, filters, from, count);
        return new ApiListResponse<ApiNewsEntryResponse>(entries.Select(e=>new ApiNewsEntryResponse(e)), totalEntries);
    }
}
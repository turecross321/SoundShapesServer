using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.News;

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
    [DocUsesFiltration<NewsFilters>]
    [DocUsesOrder<NewsOrderType>]
    [DocSummary("Lists news.")]
    public ApiListResponse<ApiNewsEntryResponse> News(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        NewsOrderType order = context.GetOrderType<NewsOrderType>() ?? NewsOrderType.CreationDate;
        NewsFilters filters = context.GetFilters<NewsFilters>(database);

        (NewsEntry[] entries, int totalEntries) = database.GetPaginatedNews(order, descending, filters, from, count);
        return new ApiListResponse<ApiNewsEntryResponse>(entries.Select(e=>new ApiNewsEntryResponse(e)), totalEntries);
    }
}
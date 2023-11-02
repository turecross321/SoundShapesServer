using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news/id/{id}")]
    [Authentication(false)]
    [DocSummary("Retrieves news entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NewsEntryNotFoundWhen)]
    [DocRouteParam("id", "News entry ID.")]
    public ApiResponse<ApiNewsEntryResponse> NewsEntryWithId(RequestContext context, GameDatabaseContext database,
        string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null)
            return ApiNotFoundError.NewsEntryNotFound;

        return ApiNewsEntryResponse.FromOld(newsEntry);
    }

    [ApiEndpoint("news")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<NewsFilters>]
    [DocUsesOrder<NewsOrderType>]
    [DocSummary("Lists news.")]
    public ApiListResponse<ApiNewsEntryResponse> News(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        NewsOrderType order = context.GetOrderType<NewsOrderType>() ?? NewsOrderType.CreationDate;
        NewsFilters filters = context.GetFilters<NewsFilters>(database);

        PaginatedList<NewsEntry> entries = database.GetPaginatedNews(order, descending, filters, from, count);
        return PaginatedList<ApiNewsEntryResponse>.SwapItems<ApiNewsEntryResponse, NewsEntry>(entries);
    }
}
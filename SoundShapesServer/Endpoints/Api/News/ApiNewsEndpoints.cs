using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
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
    public ApiResponse<ApiNewsResponse> NewsEntryWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null)
            return ApiNotFoundError.NewsEntryNotFound;
        
        return new ApiNewsResponse(newsEntry);
    }
    
    [ApiEndpoint("news"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists news.")]
    public ApiListResponse<ApiNewsResponse> News(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);
        
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

        (NewsEntry[] entries, int totalEntries) = database.GetPaginatedNews(order, descending, filters, from, count);
        return new ApiListResponse<ApiNewsResponse>(entries.Select(e=>new ApiNewsResponse(e)), totalEntries);
    }
}
using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists levels.")]
    public ApiListResponse<ApiLevelBriefResponse> GetLevels(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        LevelFilters filters = LevelHelper.GetLevelFilters(context, database);
        LevelOrderType order = LevelHelper.GetLevelOrderType(context);

        (GameLevel[] levels, int levelCount) = database.GetPaginatedLevels(order, descending, filters, from, count);
        
        return new ApiListResponse<ApiLevelBriefResponse>(levels.Select(l => new ApiLevelBriefResponse(l)), levelCount);
    }

    [ApiEndpoint("levels/id/{levelId}"), Authentication(false)]
    [DocSummary("Retrieves level with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    public ApiLevelFullResponse? GetLevelWithId(RequestContext context, GameDatabaseContext database, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level);
    }
}
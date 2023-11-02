using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints : EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<LevelFilters>]
    [DocUsesOrder<LevelOrderType>]
    [DocSummary("Lists levels.")]
    public ApiListResponse<ApiLevelBriefResponse> GetLevels(RequestContext context, GameDatabaseContext database,
        GameUser? user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        LevelFilters filters = context.GetFilters<LevelFilters>(database);
        LevelOrderType order = context.GetOrderType<LevelOrderType>() ?? LevelOrderType.CreationDate;

        PaginatedList<GameLevel> levels = database.GetPaginatedLevels(order, descending, filters, from, count, user);
        return PaginatedList<ApiLevelBriefResponse>.SwapItems<ApiLevelBriefResponse, GameLevel>(levels);
    }

    [ApiEndpoint("levels/id/{id}")]
    [Authentication(false)]
    [DocSummary("Retrieves level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiResponse<ApiLevelFullResponse> GetLevelWithId(RequestContext context, GameDatabaseContext database,
        string id, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;

        return ApiLevelFullResponse.FromOld(level);
    }
}
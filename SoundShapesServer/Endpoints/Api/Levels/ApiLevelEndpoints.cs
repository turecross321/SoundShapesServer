using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists levels.")]
    public ApiListResponse<ApiLevelBriefResponse> GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        LevelFilters filters = LevelHelper.GetLevelFilters(context, database);
        LevelOrderType order = LevelHelper.GetLevelOrderType(context);

        (GameLevel[] levels, int levelCount) = database.GetPaginatedLevels(order, descending, filters, from, count, user);
        
        return new ApiListResponse<ApiLevelBriefResponse>(levels.Select(l => new ApiLevelBriefResponse(l)), levelCount);
    }

    [ApiEndpoint("levels/id/{levelId}"), Authentication(false)]
    [DocSummary("Retrieves level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiResponse<ApiLevelFullResponse> GetLevelWithId(RequestContext context, GameDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        if (!level.HasUserAccess(user))
            return ApiNotFoundError.LevelNotFound;
            
        return new ApiLevelFullResponse(level);
    }
}
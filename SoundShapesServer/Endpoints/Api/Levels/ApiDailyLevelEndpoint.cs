using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelEndpoint : EndpointGroup
{
    [ApiEndpoint("daily"), Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<DailyLevelFilters>]
    [DocUsesOrder<DailyLevelOrderType>]
    [DocSummary("Lists levels that have been picked as daily levels.")]
    public ApiListResponse<ApiDailyLevelResponse> GetDailyLevelObjects(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        DailyLevelFilters filters = context.GetFilters<DailyLevelFilters>(database);
        DailyLevelOrderType order = context.GetOrderType<DailyLevelOrderType>() ?? DailyLevelOrderType.Date;

        (DailyLevel[] dailyLevels, int totalLevels) = database.GetPaginatedDailyLevels(order, descending, filters, from, count);
        return new ApiListResponse<ApiDailyLevelResponse>(dailyLevels.Select(d=>new ApiDailyLevelResponse(d)), totalLevels);
    }
}
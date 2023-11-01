using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelEndpoint : EndpointGroup
{
    [ApiEndpoint("daily")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<DailyLevelFilters>]
    [DocUsesOrder<DailyLevelOrderType>]
    [DocSummary("Lists levels that have been picked as daily levels.")]
    public ApiListResponse<ApiDailyLevelResponse> GetDailyLevelObjects(RequestContext context,
        GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        DailyLevelFilters filters = context.GetFilters<DailyLevelFilters>(database);
        DailyLevelOrderType order = context.GetOrderType<DailyLevelOrderType>() ?? DailyLevelOrderType.Date;

        PaginatedList<DailyLevel> daily = database.GetPaginatedDailyLevels(order, descending, filters, from, count);
        return PaginatedList<ApiDailyLevelResponse>.FromOldList<ApiDailyLevelResponse, DailyLevel>(daily);
    }
}
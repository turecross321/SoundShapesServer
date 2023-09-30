using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelEndpoint : EndpointGroup
{
    [ApiEndpoint("daily"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists levels that have been picked as daily levels.")]
    public ApiListResponse<ApiDailyLevelResponse> GetDailyLevelObjects(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        string? dateString = context.QueryString["date"];
        long? dateLong = null;
        if (dateString != null) dateLong = long.Parse(dateString);
        DateTimeOffset? date = null;
        if (dateLong != null) date = DateTimeOffset.FromUnixTimeSeconds((long)dateLong);
        
        bool? lastDate = null;
        if (bool.TryParse(context.QueryString["lastDate"], out bool lastDateTemp)) lastDate = lastDateTemp;

        DailyLevelFilters filters = new (date, lastDate);
        
        string? orderString = context.QueryString["orderBy"];
        DailyLevelOrderType order = orderString switch
        {
            "date" => DailyLevelOrderType.Date,
            _ => DailyLevelOrderType.Date
        };

        (DailyLevel[] dailyLevels, int totalLevels) = database.GetPaginatedDailyLevels(order, descending, filters, from, count);
        return new ApiListResponse<ApiDailyLevelResponse>(dailyLevels.Select(d=>new ApiDailyLevelResponse(d)), totalLevels);
    }
}
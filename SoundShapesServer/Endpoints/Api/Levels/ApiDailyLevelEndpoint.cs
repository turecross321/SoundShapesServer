using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Levels;
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
        DateTimeOffset? date = null;
        if (dateString != null) date = DateTimeOffset.Parse(dateString).Date;
        
        bool? lastDate = null;
        if (bool.TryParse(context.QueryString["lastDate"], out bool lastDateTemp)) lastDate = lastDateTemp;

        DailyLevelFilters filters = new (date, lastDate);
        
        string? orderString = context.QueryString["orderBy"];
        DailyLevelOrderType order = orderString switch
        {
            "date" => DailyLevelOrderType.Date,
            _ => DailyLevelOrderType.Date
        };

        IQueryable<DailyLevel> dailyLevels = database.GetDailyLevels(order, descending, filters);
        DailyLevel[] paginatedDailyLevels = PaginationHelper.PaginateDailyLevels(dailyLevels, from, count);
        
        return new ApiListResponse<ApiDailyLevelResponse>(paginatedDailyLevels.Select(d=>new ApiDailyLevelResponse(d)), dailyLevels.Count());
    }
}
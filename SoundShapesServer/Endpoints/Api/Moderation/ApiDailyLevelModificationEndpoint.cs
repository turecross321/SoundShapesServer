using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static System.Boolean;
using static SoundShapesServer.Helpers.DailyLevelHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiDailyLevelModificationEndpoint : EndpointGroup
{
    [ApiEndpoint("daily")]
    public ApiDailyLevelsWrapper GetDailyLevelObjects(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        string? date = context.QueryString["date"];
        bool descending = Parse(context.QueryString["descending"] ?? "true");
        
        IQueryable<DailyLevel> dailyLevels = database.GetDailyLevelObjects();
        IQueryable<DailyLevel> filteredDailyLevels = FilterDailyLevels(dailyLevels, date);
        IQueryable<DailyLevel> orderedDailyLevels =
            descending ? filteredDailyLevels.AsEnumerable().Reverse().AsQueryable() : filteredDailyLevels;

        DailyLevel[] paginatedDailyLevels = PaginationHelper.PaginateDailyLevels(orderedDailyLevels, from, count);

        return new ApiDailyLevelsWrapper(
            dailyLevels: paginatedDailyLevels.Select(t => new ApiDailyLevelResponse(t)).ToArray(),
            count: dailyLevels.Count());
    }
    
    [ApiEndpoint("daily/create", Method.Post)]
    public Response AddDailyLevel(RequestContext context, RealmDatabaseContext database, GameUser user, ApiAddDailyLevelRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        DailyLevel createdDailyLevel = database.AddDailyLevel(level, body.DateUtc);
        return new Response(new ApiDailyLevelResponse(createdDailyLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("daily/{id}/remove", Method.Post)]
    public Response RemoveDailyLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        database.RemoveDailyLevel(dailyLevel);
        return HttpStatusCode.OK;
    }
}
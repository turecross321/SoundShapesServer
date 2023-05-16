using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiDailyLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("daily")]
    public Response GetDailyLevelObjects(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        bool descending = Parse(context.QueryString["descending"] ?? "true");
        
        string? dateString = context.QueryString["date"];
        DateTimeOffset? date = null;
        if (dateString != null) date = DateTimeOffset.Parse(dateString).Date;
        
        bool? lastDate = null;
        if (TryParse(context.QueryString["lastDate"], out bool lastDateTemp)) lastDate = lastDateTemp;

        DailyLevelFilters filters = new (date, lastDate);
        
        string? orderString = context.QueryString["orderBy"];
        DailyLevelOrderType order = orderString switch
        {
            "date" => DailyLevelOrderType.Date,
            _ => DailyLevelOrderType.Date
        };

        IQueryable<DailyLevel> dailyLevels = database.GetDailyLevelObjects(order, descending, filters);
        DailyLevel[] paginatedDailyLevels = PaginationHelper.PaginateDailyLevels(dailyLevels, from, count);
        
        return new Response(new ApiDailyLevelsWrapper(paginatedDailyLevels, dailyLevels.Count()), ContentType.Json);
    }
    
    [ApiEndpoint("daily/create", Method.Post)]
    public Response AddDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiAddDailyLevelRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        DailyLevel createdDailyLevel = database.CreateDailyLevel(user, level, body.Date);
        return new Response(new ApiDailyLevelResponse(createdDailyLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("daily/id/{id}/edit", Method.Post)]
    public Response EditDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiAddDailyLevelRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        DailyLevel createdDailyLevel = database.EditDailyLevel(dailyLevel, level, body.Date);
        return new Response(new ApiDailyLevelResponse(createdDailyLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("daily/id/{id}/remove", Method.Post)]
    public Response RemoveDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        database.RemoveDailyLevel(dailyLevel);
        return HttpStatusCode.OK;
    }
}
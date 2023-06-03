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

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelManagementEndpoints : EndpointGroup
{
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

        DailyLevel createdDailyLevel = database.EditDailyLevel(dailyLevel, user, level, body.Date);
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
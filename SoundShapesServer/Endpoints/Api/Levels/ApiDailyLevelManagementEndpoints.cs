using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("daily/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Picks level as a daily level.")]
    public Response AddDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateDailyLevelRequest body)
    {
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        DailyLevel createdDailyLevel = database.CreateDailyLevel(user, level, DateTimeOffset.FromUnixTimeSeconds(body.Date));
        return new Response(new ApiDailyLevelResponse(createdDailyLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("daily/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits daily level pick with specified ID.")]
    public Response EditDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiCreateDailyLevelRequest body)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        DailyLevel createdDailyLevel = database.EditDailyLevel(dailyLevel, user, level, DateTimeOffset.FromUnixTimeSeconds(body.Date));
        return new Response(new ApiDailyLevelResponse(createdDailyLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("daily/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Removes daily level pick with specified ID.")]
    public Response RemoveDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        database.RemoveDailyLevel(dailyLevel);
        return HttpStatusCode.OK;
    }
}
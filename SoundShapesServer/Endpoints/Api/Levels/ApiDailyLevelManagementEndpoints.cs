using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiDailyLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("daily/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Picks level as a daily level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiResponse<ApiDailyLevelResponse> AddDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiCreateDailyLevelRequest body)
    {
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        DailyLevel createdDailyLevel = database.CreateDailyLevel(user, level, DateTimeOffset.FromUnixTimeSeconds(body.Date));
        return new ApiDailyLevelResponse(createdDailyLevel);
    }
    
    [ApiEndpoint("daily/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits daily level pick with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.DailyLevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiResponse<ApiDailyLevelResponse> EditDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id, ApiCreateDailyLevelRequest body)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) 
            return ApiNotFoundError.DailyLevelNotFound;
        
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return ApiNotFoundError.LevelNotFound;

        DailyLevel createdDailyLevel = database.EditDailyLevel(dailyLevel, user, level, DateTimeOffset.FromUnixTimeSeconds(body.Date));
        return new ApiDailyLevelResponse(createdDailyLevel);
    }
    
    [ApiEndpoint("daily/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Removes daily level pick with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.DailyLevelNotFoundWhen)]
    public ApiOkResponse RemoveDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) 
            return ApiNotFoundError.DailyLevelNotFound;
        
        database.RemoveDailyLevel(dailyLevel);
        return new ApiOkResponse();
    }
}
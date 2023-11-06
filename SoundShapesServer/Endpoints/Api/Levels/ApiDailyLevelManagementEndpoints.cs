using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
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
    [ApiEndpoint("daily/create", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Picks level as a daily level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiResponse<ApiDailyLevelResponse> AddDailyLevel(RequestContext context, GameDatabaseContext database,
        GameUser user, ApiCreateDailyLevelRequest body)
    {
        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        DailyLevel createdDailyLevel =
            database.CreateDailyLevel(user, level, body.Date);
        return ApiDailyLevelResponse.FromOld(createdDailyLevel);
    }

    [ApiEndpoint("daily/id/{id}/edit", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits daily level pick with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.DailyLevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocRouteParam("id", "Daily level object ID.")]
    public ApiResponse<ApiDailyLevelResponse> EditDailyLevel(RequestContext context, GameDatabaseContext database,
        GameUser user, string id, ApiCreateDailyLevelRequest body)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null)
            return ApiNotFoundError.DailyLevelNotFound;

        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return ApiNotFoundError.LevelNotFound;

        DailyLevel createdDailyLevel =
            database.EditDailyLevel(dailyLevel, user, level, body.Date);
        return ApiDailyLevelResponse.FromOld(createdDailyLevel);
    }

    [ApiEndpoint("daily/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Removes daily level pick with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.DailyLevelNotFoundWhen)]
    [DocRouteParam("id", "Daily level object ID.")]
    public ApiOkResponse RemoveDailyLevel(RequestContext context, GameDatabaseContext database, GameUser user,
        string id)
    {
        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null)
            return ApiNotFoundError.DailyLevelNotFound;

        database.RemoveDailyLevel(dailyLevel);
        return new ApiOkResponse();
    }
}
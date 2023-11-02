using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Moderation;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api;

public class ApiPunishmentManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("punishments/create", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Punishes user.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiMethodNotAllowedError), ApiMethodNotAllowedError.PunishYourselfWhen)]
    public ApiResponse<ApiPunishmentResponse> CreatePunishment(RequestContext context, GameDatabaseContext database,
        GameUser user, ApiPunishRequest body)
    {
        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null)
            return ApiNotFoundError.UserNotFound;

        if (userToPunish.Id == user.Id)
            return ApiMethodNotAllowedError.PunishYourself;

        Punishment createdPunishment = database.CreatePunishment(user, userToPunish, body);
        return ApiPunishmentResponse.FromOld(createdPunishment);
    }

    [ApiEndpoint("punishments/id/{id}/edit", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Edits punishment with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.PunishmentNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiMethodNotAllowedError), ApiMethodNotAllowedError.PunishYourselfWhen)]
    [DocRouteParam("id", "Punishment ID.")]
    public ApiResponse<ApiPunishmentResponse> EditPunishment(RequestContext context, GameDatabaseContext database,
        GameUser user, string id,
        ApiPunishRequest body)
    {
        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null)
            return ApiNotFoundError.PunishmentNotFound;

        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null)
            return ApiNotFoundError.UserNotFound;

        if (userToPunish.Id == user.Id)
            return ApiMethodNotAllowedError.PunishYourself;

        Punishment editedPunishment = database.EditPunishment(user, punishment, userToPunish, body);
        return ApiPunishmentResponse.FromOld(editedPunishment);
    }

    [ApiEndpoint("punishments/id/{id}/revoke", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Revokes punishment with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.PunishmentNotFoundWhen)]
    [DocRouteParam("id", "Punishment ID.")]
    public ApiResponse<ApiPunishmentResponse> RevokePunishment(RequestContext context, GameDatabaseContext database,
        GameUser user, string id)
    {
        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null)
            return ApiNotFoundError.PunishmentNotFound;

        database.RevokePunishment(punishment);
        return ApiPunishmentResponse.FromOld(punishment);
    }

    [ApiEndpoint("punishments")]
    [DocUsesPageData]
    [DocUsesFiltration<PunishmentFilters>]
    [DocUsesOrder<PunishmentOrderType>]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Lists punishments.")]
    public ApiListResponse<ApiPunishmentResponse> GetPunishments(RequestContext context, GameDatabaseContext database,
        GameUser user)
    {
        (int from, int count, bool descending) = context.GetPageData();

        PunishmentFilters filters = context.GetFilters<PunishmentFilters>(database);
        PunishmentOrderType order = context.GetOrderType<PunishmentOrderType>() ?? PunishmentOrderType.CreationDate;

        PaginatedList<Punishment> punishments =
            database.GetPaginatedPunishments(order, descending, filters, from, count);
        return PaginatedList<ApiPunishmentResponse>.ToResponses<ApiPunishmentResponse, Punishment>(punishments);
    }
}
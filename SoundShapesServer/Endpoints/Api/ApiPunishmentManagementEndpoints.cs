using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
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
    public ApiResponse<ApiPunishmentResponse> CreatePunishment(RequestContext context, GameDatabaseContext database, GameUser user, ApiPunishRequest body)
    {
        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null) 
            return ApiNotFoundError.UserNotFound;

        if (userToPunish.Id == user.Id) 
            return ApiMethodNotAllowedError.PunishYourself;
        
        Punishment createdPunishment = database.CreatePunishment(user, userToPunish, body);
        return new ApiPunishmentResponse(createdPunishment);
    }

    [ApiEndpoint("punishments/id/{id}/edit", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Edits punishment with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.PunishmentNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiMethodNotAllowedError), ApiMethodNotAllowedError.PunishYourselfWhen)]
    public ApiResponse<ApiPunishmentResponse> EditPunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id,
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
        return new ApiPunishmentResponse(editedPunishment);
    }

    [ApiEndpoint("punishments/id/{id}/revoke", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Revokes punishment with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.PunishmentNotFoundWhen)]
    public ApiResponse<ApiPunishmentResponse> RevokePunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null) 
            return ApiNotFoundError.PunishmentNotFound;
        
        database.RevokePunishment(punishment);
        return new ApiPunishmentResponse(punishment);
    }
    
    [ApiEndpoint("punishments")]
    [DocUsesPageData]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Lists punishments.")]
    public ApiListResponse<ApiPunishmentResponse> GetPunishments(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        string? authorString = context.QueryString["author"];
        GameUser? author = null;
        if (authorString != null)
            author = database.GetUserWithId(authorString);
        
        string? recipientString = context.QueryString["recipient"];
        GameUser? recipient = null;
        if (recipientString != null)
            recipient = database.GetUserWithId(recipientString);

        bool? revoked = null;
        if (bool.TryParse(context.QueryString["revoked"], out bool revokedTemp)) revoked = revokedTemp;

        PunishmentFilters filters = new (author, recipient, revoked);
        (Punishment[] punishments, int totalPunishments) = database.GetPaginatedPunishments(PunishmentOrderType.CreationDate, descending, filters, from, count);

        return new ApiListResponse<ApiPunishmentResponse>(punishments.Select(p=>new ApiPunishmentResponse(p)), totalPunishments);
    }
}
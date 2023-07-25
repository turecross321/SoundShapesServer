using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api;

public class ApiPunishmentManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("punishments/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Punishes user.")]
    public Response CreatePunishment(RequestContext context, GameDatabaseContext database, GameUser user, ApiPunishRequest body)
    {
        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null) return HttpStatusCode.NotFound;

        if (userToPunish.Id == user.Id) return HttpStatusCode.MethodNotAllowed;
        
        Punishment createdPunishment = database.CreatePunishment(user, userToPunish, body);
        return new Response(new ApiPunishmentResponse(createdPunishment), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("punishments/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Edits punishment with specified ID.")]
    public Response EditPunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id,
        ApiPunishRequest body)
    {
        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null) return HttpStatusCode.NotFound;

        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null) return HttpStatusCode.NotFound;
        
        if (userToPunish.Id == user.Id) return HttpStatusCode.MethodNotAllowed;

        Punishment editedPunishment = database.EditPunishment(user, punishment, userToPunish, body);
        return new Response(new ApiPunishmentResponse(editedPunishment), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("punishments/id/{id}/revoke", Method.Post)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Revokes punishment with specified ID.")]
    public Response RevokePunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null) return HttpStatusCode.NotFound;
        
        database.RevokePunishment(punishment);
        return new Response(new ApiPunishmentResponse(punishment), ContentType.Json);
    }
    
    [ApiEndpoint("punishments")]
    [DocUsesPageData]
    [MinimumPermissions(PermissionsType.Administrator)]
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
        (Punishment[] punishments, int totalPunishments) = database.GetPunishments(PunishmentOrderType.CreationDate, descending, filters, from, count);

        return new ApiListResponse<ApiPunishmentResponse>(punishments.Select(p=>new ApiPunishmentResponse(p)), totalPunishments);
    }
}
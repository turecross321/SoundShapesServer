using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiPunishmentManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("punishments/create", Method.Post)]
    public Response PunishUser(RequestContext context, GameDatabaseContext database, GameUser user, ApiPunishRequest body)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null) return HttpStatusCode.NotFound;

        if (userToPunish.Id == user.Id) return HttpStatusCode.MethodNotAllowed;
        
        Punishment createdPunishment = database.PunishUser(user, userToPunish, body);
        return new Response(new ApiPunishmentResponse(createdPunishment), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("punishments/{id}/edit", Method.Post)]
    public Response EditPunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id,
        ApiPunishRequest body)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null) return HttpStatusCode.NotFound;

        GameUser? userToPunish = database.GetUserWithId(body.UserId);
        if (userToPunish == null) return HttpStatusCode.NotFound;
        
        if (userToPunish.Id == user.Id) return HttpStatusCode.MethodNotAllowed;

        Punishment editedPunishment = database.EditPunishment(punishment, userToPunish, body);
        return new Response(new ApiPunishmentResponse(editedPunishment), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("punishments/{id}/revoke", Method.Post)]
    public Response RevokePunishment(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        Punishment? punishment = database.GetPunishmentWithId(id);
        if (punishment == null) return HttpStatusCode.NotFound;
        
        database.RevokePunishment(punishment);
        return HttpStatusCode.OK;
    }
    
    [ApiEndpoint("punishments")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public ApiPunishmentsWrapper? GetPunishments(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return null;
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        string? authorString = context.QueryString["author"];
        GameUser? author = null;
        if (authorString != null)
            author = database.GetUserWithId(authorString);
        
        string? recipientString = context.QueryString["recipient"];
        GameUser? recipient = null;
        if (recipientString != null)
            recipient = database.GetUserWithId(recipientString);

        bool? revoked = null;
        if (TryParse(context.QueryString["revoked"], out bool revokedTemp)) revoked = revokedTemp;
        
        bool descending = Parse(context.QueryString["descending"] ?? "true");

        PunishmentFilters filters = new (author, recipient, revoked);
        (Punishment[] punishments, int totalPunishments) = database.GetPunishments(descending, filters, from, count);

        return new ApiPunishmentsWrapper(punishments, totalPunishments);
    }
}
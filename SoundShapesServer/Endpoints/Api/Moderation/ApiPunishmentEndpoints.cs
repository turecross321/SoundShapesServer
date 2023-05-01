using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Moderation;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiPunishmentEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}/punish", Method.Post)]
    public Response PunishUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id, ApiPunishRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToPunish = database.GetUserWithId(id);
        if (userToPunish == null) return HttpStatusCode.NotFound;
        
        database.PunishUser(user, body);
        return HttpStatusCode.Created;
    }

    // todo: dismiss punishments
    
    [ApiEndpoint("punishments")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public ApiPunishmentsWrapper? GetPunishments(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return null;
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        IQueryable<Punishment> punishments = database.GetPunishments();
        return new ApiPunishmentsWrapper(punishments, from, count);
    }
}
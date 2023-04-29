using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiPunishmentEndpoints : EndpointGroup
{
    [ApiEndpoint("user/{id}/punish", Method.Post)]
    public Response PunishUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id, PunishRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameUser? userToPunish = database.GetUserWithId(id);
        if (userToPunish == null) return HttpStatusCode.NotFound;
        
        database.PunishUser(user, body);
        return HttpStatusCode.Created;
    }
}
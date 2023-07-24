using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Api.Account;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    private readonly Response _invalidCredentialsResponse = new ("The email address or password was incorrect.", ContentType.Plaintext, HttpStatusCode.Forbidden);
    
    [ApiEndpoint("account/logIn", Method.Post), Authentication(false)]
    public Response Login(RequestContext context, GameDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return _invalidCredentialsResponse;

        if (!database.ValidatePassword(user, body.PasswordSha512)) return _invalidCredentialsResponse;

        IQueryable<Punishment> activeBans = GetActiveUserBans(user);
        SessionType sessionType = activeBans.Any() ? SessionType.Banned : SessionType.Api;
        GameSession session = database.CreateSession(user, sessionType, PlatformType.Api);
        
        return new Response(new ApiSessionResponse(session, activeBans), ContentType.Json);
    }

    [ApiEndpoint("account/logOut", Method.Post)]
    [DocSummary("Revokes the session used to make this request.")]
    public Response Logout(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        database.RemoveSession(session);
        return HttpStatusCode.OK;
    }
}
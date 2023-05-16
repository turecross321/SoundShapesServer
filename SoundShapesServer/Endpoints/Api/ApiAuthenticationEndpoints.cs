using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    private readonly Response _invalidCredentialsResponse = new ("The email address or password was incorrect.", ContentType.Plaintext, HttpStatusCode.Forbidden);
    
    [ApiEndpoint("account/login", Method.Post)]
    [Authentication(false)]
    public Response Login(RequestContext context, GameDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return _invalidCredentialsResponse;

        if (!database.ValidatePassword(user, body.PasswordSha512)) return _invalidCredentialsResponse;

        Punishment? ban = IsUserBanned(user);
        GameSession session = database.CreateSession(context, user, ban == null ? SessionType.Api : SessionType.Banned);
        
        return new Response(new ApiSessionResponse(session, ban), ContentType.Json);
    }

    [ApiEndpoint("account/logout", Method.Post)]
    public Response Logout(RequestContext context, GameDatabaseContext database, GameSession session)
    {
        database.RemoveSession(session);
        return HttpStatusCode.OK;
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.PunishmentHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    public const int WorkFactor = 10;

    [ApiEndpoint("account/login", Method.Post)]
    [Authentication(false)]
    public Response Login(RequestContext context, RealmDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null)
        {
            return new Response("The email address or password was incorrect.", ContentType.Json, HttpStatusCode.Forbidden);
        }

        if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
        {
            database.SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor));
        }

        if (BCrypt.Net.BCrypt.Verify(body.PasswordSha512, user.PasswordBcrypt) == false)
        {
            return new Response("The email address or password was incorrect.", ContentType.Json, HttpStatusCode.Forbidden);
        }
        
        // Check if user is banned
        Punishment[] bans = GetUsersPunishmentsOfType(user, PunishmentType.Ban);
        if (bans.Length > 0)
        {
            Punishment? longestBan = bans.MaxBy(p => p.ExpiresAt);
            if (longestBan == null) return new Response("User is banned.", ContentType.Json, HttpStatusCode.Forbidden);

            string banString = "User is banned. Expires at " + longestBan.ExpiresAt + ".\n" +
                               "Reason: " + longestBan.Reason;
            return new Response(banString, ContentType.Json, HttpStatusCode.Forbidden);
        }

        GameSession session = database.GenerateSessionForUser(context, user, SessionType.Api);

        ApiAuthenticationResponse response = new(session);

        return new Response(response, ContentType.Json);
    }

    [ApiEndpoint("account/logout", Method.Post)]
    public Response Logout(RequestContext context, RealmDatabaseContext database, GameSession session)
    {
        database.RemoveSession(session);
        return HttpStatusCode.OK;
    }
}
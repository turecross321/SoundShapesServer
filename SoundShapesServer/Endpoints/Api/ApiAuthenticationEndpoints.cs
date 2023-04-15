using System.Net;
using System.Text.RegularExpressions;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    private const int WorkFactor = 14;

    [ApiEndpoint("login", Method.Post)]
    [Authentication(false)]
    public Response Authenticate(RequestContext context, RealmDatabaseContext database, ApiLoginRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null)
        {
            return new Response(new ApiErrorResponse {Reason = "The username or password was incorrect."}, ContentType.Json, HttpStatusCode.Forbidden);   
        }

        if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
        {
            database.SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor));
        }

        if (BCrypt.Net.BCrypt.Verify(body.PasswordSha512, user.PasswordBcrypt) == false)
        {
            return new Response(new ApiErrorResponse {Reason = "The username or password was incorrect."}, ContentType.Json, HttpStatusCode.Forbidden);
        }

        GameSession session = database.GenerateSessionForUser(user, (int)TypeOfSession.API);

        ApiAuthenticationResponse response = new()
        {
            Id = session.Id,
            UserId = user.Id,
            ExpiresAt = session.ExpiresAt
        };

        return new Response(response, ContentType.Json);
    }

    [ApiEndpoint("setEmail", Method.Post)]
    public Response SetUserEmail(RequestContext context, RealmDatabaseContext database, ApiSetEmailRequest body, GameSession token)
    {
        if (token.SessionType != (int)TypeOfSession.SetEmail) return HttpStatusCode.Unauthorized;

        GameUser user = token.User;

        if (user.HasFinishedRegistration)
        {
            return new Response(new ApiErrorResponse {Reason = "User has already finished registration."}, ContentType.Json, HttpStatusCode.Forbidden);
        }

        database.SetUserEmail(user, body.Email, token);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("setPassword", Method.Post)]
    public Response SetUserPassword(RequestContext context, RealmDatabaseContext database, ApiSetPasswordRequest body, GameSession token)
    {
        if (token.SessionType != (int)TypeOfSession.SetPassword) return HttpStatusCode.Unauthorized;

        GameUser user = token.User;
        
        if (user.HasFinishedRegistration) return HttpStatusCode.Forbidden;

            string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor);
        
        database.SetUserPassword(user, passwordBcrypt, token);

        return HttpStatusCode.Created;
    }
}
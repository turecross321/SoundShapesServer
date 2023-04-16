using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;
using static SoundShapesServer.Helpers.IpHelper;

namespace SoundShapesServer.Endpoints.Api;

public partial class ApiAuthenticationEndpoints : EndpointGroup
{
    [GeneratedRegex("^[a-f0-9]{128}$")]
    private static partial Regex Sha512Regex();
    
    private const int WorkFactor = 14;

    [ApiEndpoint("login", Method.Post)]
    [Authentication(false)]
    public Response Login(RequestContext context, RealmDatabaseContext database, ApiLoginRequest body)
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
        
        GameSession session = database.GenerateSessionForUser(context, user, (int)TypeOfSession.API);

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

        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.Email, out MailAddress? mailAddress) == false)
        {
            return new Response(new ApiErrorResponse {Reason = "Invalid Email."}, ContentType.Json, HttpStatusCode.Forbidden);
        }
        
        database.SetUserEmail(user, body.Email, token);

        string passwordSessionId = GenerateSimpleSessionId(database);
        GameSession passwordSession = database.GenerateSessionForUser(context, user, (int)TypeOfSession.SetPassword, 600, passwordSessionId); // 10 minutes
        // Todo: Send PasswordSession to mail address

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("setPassword", Method.Post)]
    public Response SetUserPassword(RequestContext context, RealmDatabaseContext database, ApiSetPasswordRequest body, GameSession token)
    {
        if (token.SessionType != (int)TypeOfSession.SetPassword) return HttpStatusCode.Unauthorized;

        GameUser user = token.User;
        
        if (user.HasFinishedRegistration) return HttpStatusCode.Forbidden;
        
        if (body.PasswordSha512.Length != 128 || !Sha512Regex().IsMatch(body.PasswordSha512))
            return new Response("Password is definitely not SHA512. Please hash the password.",
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor);
        
        database.SetUserPassword(user, passwordBcrypt, token);

        return HttpStatusCode.Created;
    }
}
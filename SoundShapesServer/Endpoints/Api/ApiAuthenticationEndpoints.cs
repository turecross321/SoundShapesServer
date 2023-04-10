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
    public Response Authenticate(RequestContext context, RealmDatabaseContext database, ApiAuthenticationRequest body)
    {
        GameUser? user = database.GetUserWithUsername(body.Username);
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

        Session session = database.GenerateSessionForUser(user, PlatformType.API);

        ApiAuthenticationResponse response = new()
        {
            Id = session.Id,
            UserId = user.Id,
            ExpiresAt = session.ExpiresAt
        };

        return new Response(response, ContentType.Json);
    }

    [ApiEndpoint("register", Method.Post)]
    [Authentication(false)]
    public Response Register(RequestContext context, RealmDatabaseContext database, ApiAuthenticationRequest body)
    {
        GameUser? user = database.GetUserWithUsername(body.Username);
        if (user != null)
        {
            return new Response(new ApiErrorResponse {Reason = "Username is already taken."}, ContentType.Json, HttpStatusCode.Forbidden);   
        }

        database.CreateUser(body.Username, BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor));

        return HttpStatusCode.OK;
    }
}
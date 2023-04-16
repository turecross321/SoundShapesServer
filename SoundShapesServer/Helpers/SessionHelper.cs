using System.Net;
using System.Text.RegularExpressions;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class SessionHelper
{
    private const string IdCharacters = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int IdLength = 8;
        
    public static string GenerateSimpleSessionId(RealmDatabaseContext database)
    {
        Random r = new Random();
        string id = "";
        for (int i = 0; i < IdLength; i++)
        {
            id += IdCharacters[r.Next(IdCharacters.Length - 1)];
        }

        if (database.GetSessionWithSessionId(id) == null) return id; // Return if Id has not been used before
        return GenerateSimpleSessionId(database); // Generate new Id if it already exists   
    }
    public static GameSessionResponse SessionToSessionResponse(GameSession session)
    {
        GameSessionResponse gameSessionResponse = new GameSessionResponse
        {
            ExpirationDate = session.ExpiresAt.ToUnixTimeMilliseconds(),
            Id = session.Id,
            User = new SessionUserResponse
            {
                Username = session.User.Username,
                Id = session.User.Id
            }
        };

        return gameSessionResponse;
    }
    public static Response SessionResponseToResponse(RequestContext context, GameSessionResponse gameSessionResponse)
    {
        GameSessionWrapper responseWrapper = new ()
        {
            GameSession = gameSessionResponse
        };
        
        Console.WriteLine($"{gameSessionResponse.User.Username} has logged in.");

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={gameSessionResponse.Id};Version=1;Path=/");
        context.ResponseHeaders.Add("x-otg-identity-displayname", gameSessionResponse.User.Username);
        context.ResponseHeaders.Add("x-otg-identity-personid", gameSessionResponse.User.Id);
        context.ResponseHeaders.Add("x-otg-identity-sessionid", gameSessionResponse.Id);
        
        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }

    public static readonly int[] GameSessionTypes =
    {
        (int)TypeOfSession.PS3,
        (int)TypeOfSession.PS4,
        (int)TypeOfSession.PSVita,
        (int)TypeOfSession.RPCS3
    };

    public static bool IsSessionAllowedToAccessEndpoint(GameSession session, string uriPath)
    {
        // If Session is a Game Session, let it only access Game endpoints
        if (GameSessionTypes.Contains(session.SessionType))
        {
            if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute)) return true;
            if (uriPath.StartsWith("/identity/")) return true;

            return false;
        }

        // If Session is an API Session, let it only access api endpoints
        if (session.SessionType == (int)TypeOfSession.API)
        {
            if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute)) return true;

            return false;
        }

        // If Session is a SetEmail Session, let it only access the setEmail endpoint
        if (session.SessionType == (int)TypeOfSession.SetEmail)
        {
            if (uriPath == ApiEndpointAttribute.BaseRoute + "setEmail") return true;

            return false;
        }

        // If Session is a SetPassword Session, let it only access the SetPassword endpoint
        if (session.SessionType == (int)TypeOfSession.SetPassword)
        {
            if (uriPath == ApiEndpointAttribute.BaseRoute + "setPassword") return true;

            return false;
        }

        // If Session is an Unauthorized Session, let it only access the Eula Endpoint 
        if (session.SessionType == (int)TypeOfSession.Unauthorized)
        {
            string eulaUrlPattern = "/otg/[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get";

            Match match = Regex.Match(uriPath, eulaUrlPattern);

            return match.Success || uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello";
        }

        return false;
    }
}
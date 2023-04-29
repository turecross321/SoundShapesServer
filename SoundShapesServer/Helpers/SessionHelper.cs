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
    public static string GenerateEmailSessionId(RealmDatabaseContext database) =>
        GenerateSimpleSessionId(database, "123456789", 8);
    public static string GeneratePasswordSessionId(RealmDatabaseContext database) =>
        GenerateSimpleSessionId(database, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8);
    public static string GenerateAccountRemovalSessionId(RealmDatabaseContext database) => GenerateSimpleSessionId(database,
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 8);
    private static string GenerateSimpleSessionId(RealmDatabaseContext database, string idCharacters, int idLength)
    {
        Random r = new Random();
        string id = "";
        for (int i = 0; i < idLength; i++)
        {
            id += idCharacters[r.Next(idCharacters.Length - 1)];
        }

        if (database.GetSessionWithSessionId(id) == null) return id; // Return if Id has not been used before
        return GenerateSimpleSessionId(database, idCharacters, idLength); // Generate new Id if it already exists   
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

    public static bool IsSessionAllowedToAccessEndpoint(GameSession session, string uriPath)
    {
        if (uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello")
        {
            // If Session is an Unauthorized Session, let it only access the Eula Endpoint 
            if (session.SessionType == (int)SessionType.Unauthorized) return true;
        }
        else if (Regex.IsMatch(uriPath, "^/otg/[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get$"))
        {
            // If Session is an Unauthorized Session, let it only access the Eula Endpoint 
            if (session.SessionType == (int)SessionType.Unauthorized) return true;
        }
        else if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute))
        {
            // If Session is a Game Session, let it only access Game endpoints
            if (session.SessionType == (int)SessionType.Game) return true;
        }
        else if (uriPath == ApiEndpointAttribute.BaseRoute + "account/setEmail")
        {
            // If Session is a SetEmail Session, let it only access the setEmail endpoint
            if (session.SessionType == (int)SessionType.SetEmail) return true;

            return false;
        }
        else if (uriPath == ApiEndpointAttribute.BaseRoute + "account/setPassword")
        {
            // If Session is a SetPassword Session, let it only access the SetPassword endpoint
            if (session.SessionType == (int)SessionType.SetPassword) return true;

            return false;
        }
        else if (uriPath == ApiEndpointAttribute.BaseRoute + "account/remove")
        {
            // If Session is a RemoveAccount Session, let it only access the Remove endpoint
            if (session.SessionType == (int)SessionType.RemoveAccount) return true;

            return false;
        }
        else if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            // If Session is an API Session, let it only access api endpoints
            if (session.SessionType == (int)SessionType.API) return true;
        }

        return false;
    }
}
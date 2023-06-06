using System.Text.RegularExpressions;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Helpers;

public static class SessionHelper
{
    public static string GenerateEmailSessionId(GameDatabaseContext database) =>
        GenerateSimpleSessionId(database, "123456789", 8);
    // ReSharper disable StringLiteralTypo    
    public static string GeneratePasswordSessionId(GameDatabaseContext database) =>
        GenerateSimpleSessionId(database, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8);
    public static string GenerateAccountRemovalSessionId(GameDatabaseContext database) => GenerateSimpleSessionId(database,
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 8);
    // ReSharper restore StringLiteralTypo
    private static string GenerateSimpleSessionId(GameDatabaseContext database, string idCharacters, int idLength)
    {
        Random r = new();
        string id = "";
        for (int i = 0; i < idLength; i++)
        {
            id += idCharacters[r.Next(idCharacters.Length - 1)];
        }

        if (database.GetSessionWithSessionId(id) == null) return id; // Return if Id has not been used before
        return GenerateSimpleSessionId(database, idCharacters, idLength); // Generate new Id if it already exists   
    }

    public static bool IsSessionAllowedToAccessEndpoint(GameSession session, string uriPath)
    {
        if (uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello" ||
            Regex.IsMatch(uriPath, $"^{GameEndpointAttribute.BaseRoute}[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get$")
           )
        {
            if (session.SessionType is SessionType.GameUnAuthorized or SessionType.Banned) return true;
        }
        if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute) || uriPath.StartsWith("/identity/"))
        {
            // If Session is a Game Session, let it only access Game endpoints
            if (session.SessionType == SessionType.Game) return true;
        }
        switch (uriPath)
        {
            // If Session is a SetEmail Session, let it only access the setEmail endpoint
            case ApiEndpointAttribute.BaseRoute + "account/setEmail" when session.SessionType == SessionType.SetEmail:
                return true;
            // If Session is a SetPassword Session, let it only access the SetPassword endpoint
            case ApiEndpointAttribute.BaseRoute + "account/setPassword" when session.SessionType == SessionType.SetPassword:
                return true;
            // If Session is a RemoveAccount Session, let it only access the Remove endpoint
            case ApiEndpointAttribute.BaseRoute + "account/remove" when session.SessionType == SessionType.RemoveAccount:
                return true;
        }

        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            // If Session is an API Session, let it only access api endpoints
            if (session.SessionType == SessionType.Api) return true;
        }

        if (uriPath == ApiEndpointAttribute.BaseRoute + "account/sendRemovalSession")
        {
            if (session.SessionType == SessionType.Banned)
                return true;
        }

        return false;
    }
}
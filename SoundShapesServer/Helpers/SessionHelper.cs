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
    // This is used for occasions where the user has to type the session id manually, and giving them a
    // SHA512 would be pretty inconvenient
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
            // This is for the EULA endpoint, and we use regex here to support all platforms and languages
            Regex.IsMatch(uriPath, $"^{GameEndpointAttribute.BaseRoute}[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get$"))
        {
            if (session.SessionType is SessionType.GameUnAuthorized or SessionType.Banned) return true;
        }
        if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute) || uriPath.StartsWith("/identity/"))
        {
            // If Session is a Game Session, let it only access Game endpoints
            if (session.SessionType == SessionType.Game) return true;
        }

        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute + "account/"))
        {
            switch (uriPath)
            {
                case ApiEndpointAttribute.BaseRoute + "account/setEmail":
                    if (session.SessionType == SessionType.SetEmail) return true;
                    return false;
                case ApiEndpointAttribute.BaseRoute + "account/setPassword":
                    if (session.SessionType == SessionType.SetPassword) return true;
                    return false;
                case ApiEndpointAttribute.BaseRoute + "account/remove":
                    if (session.SessionType == SessionType.RemoveAccount) return true;
                    return false;
                case ApiEndpointAttribute.BaseRoute + "account/sendRemovalSession":
                    if (session.SessionType == SessionType.Banned)
                        return true;
                    // no return false here because you don't have to be banned to remove an account
                    break;
            }
        }
        
        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            if (session.SessionType == SessionType.Api) return true;
        }

        return false;
    }
}
using System.Text.RegularExpressions;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
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
            if (session.SessionType is (int)SessionType.Unauthorized or (int)SessionType.Banned) return true;
        }
        if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute) || uriPath.StartsWith("/identity/"))
        {
            // If Session is a Game Session, let it only access Game endpoints
            if (session.SessionType == (int)SessionType.Game) return true;
        }
        if (uriPath == ApiEndpointAttribute.BaseRoute + "account/setEmail")
        {
            // If Session is a SetEmail Session, let it only access the setEmail endpoint
            if (session.SessionType == (int)SessionType.SetEmail) return true;

            return false;
        }

        if (uriPath == ApiEndpointAttribute.BaseRoute + "account/setPassword")
        {
            // If Session is a SetPassword Session, let it only access the SetPassword endpoint
            if (session.SessionType == (int)SessionType.SetPassword) return true;

            return false;
        }

        if (uriPath == ApiEndpointAttribute.BaseRoute + "account/remove")
        {
            // If Session is a RemoveAccount Session, let it only access the Remove endpoint
            if (session.SessionType == (int)SessionType.RemoveAccount) return true;

            return false;
        }
        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            // If Session is an API Session, let it only access api endpoints
            if (session.SessionType == (int)SessionType.Api) return true;
        }

        if (uriPath == ApiEndpointAttribute.BaseRoute + "account/sendRemovalSession")
        {
            if (session.SessionType == (int)SessionType.Banned)
                return true;
        }

        return false;
    }
}
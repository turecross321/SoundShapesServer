using System.Text.RegularExpressions;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Helpers;

public static partial class SessionHelper
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

        if (database.GetSessionWithId(id) == null) return id; // Return if Id has not been used before
        return GenerateSimpleSessionId(database, idCharacters, idLength); // Generate new Id if it already exists   
    }

    [GeneratedRegex("^/otg/[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get$")]
    private static partial Regex EulaRegex();
    
    public static bool IsSessionAllowedToAccessEndpoint(GameSession session, string uriPath)
    {
        if (uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello"
            || EulaRegex().IsMatch(uriPath)
            && session.SessionType == SessionType.GameUnAuthorized)
        {
            return true;
        }
        if (uriPath.StartsWith(GameEndpointAttribute.BaseRoute) 
            || uriPath.StartsWith("/identity/")
            && session.SessionType == SessionType.Game)
        { 
            return true;
        }
        
        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute) && session.SessionType == SessionType.Api)
        {
            return true;
        }

        return false;
    }
}
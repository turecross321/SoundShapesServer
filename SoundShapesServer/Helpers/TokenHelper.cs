using System.Text.RegularExpressions;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Helpers;

public static partial class TokenHelper
{
    public static string GenerateSimpleTokenId(GameDatabaseContext database, string idCharacters, int idLength, TokenType type)
    {
        Random r = new();
        string id = "";
        for (int i = 0; i < idLength; i++)
        {
            id += idCharacters[r.Next(idCharacters.Length - 1)];
        }

        if (database.GetTokenWithId(id, type) == null) return id; // Return if Id has not been used before
        return GenerateSimpleTokenId(database, idCharacters, idLength, type); // Generate new Id if it already exists   
    }

    [GeneratedRegex("^/otg/[a-zA-Z0-9]+/[A-Z]+/[a-zA-Z0-9_]+/~eula.get$")]
    private static partial Regex EulaRegex();
    
    public static bool IsTokenAllowedToAccessEndpoint(GameToken token, string uriPath)
    {
        if (uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello"
            || EulaRegex().IsMatch(uriPath)
            && token.TokenType == TokenType.GameUnAuthorized)
        {
            return true;
        }
        if ((uriPath.StartsWith(GameEndpointAttribute.BaseRoute) 
            || uriPath.StartsWith("/identity/"))
            && token.TokenType == TokenType.GameAccess)
        { 
            return true;
        }
        
        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute) && token.TokenType == TokenType.ApiAccess)
        {
            return true;
        }

        return false;
    }
}
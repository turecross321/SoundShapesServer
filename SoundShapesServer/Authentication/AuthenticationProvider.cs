using Bunkum.Listener.Request;
using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Authentication;

public class AuthenticationProvider : IAuthenticationProvider<GameToken>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        GameUser? user = AuthenticateToken(request, db)?.User;
        if (user == null) return null;

        user.RateLimitUserId = user.Id;
        return user;
    }

    public GameToken? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        // check the session header. this is what the game uses
        string? tokenId = request.RequestHeaders["X-OTG-Identity-SessionId"];

        // check the authorization header. this is what the api uses.
        tokenId ??= request.RequestHeaders["Authorization"];

        // we dont have a token if null, so bail 
        if (tokenId == null) return null;

        GameDatabaseContext database = (GameDatabaseContext)db.Value;

        GameToken? token = database.GetTokenWithId(tokenId, null);
        if (token == null) return null;

        if (token.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveToken(token);
            return null;
        }

        string uriPath = request.Uri.AbsolutePath;
        
        if (uriPath == GameEndpointAttribute.BaseRoute + "~identity:*.hello"
            || TokenHelper.EulaRegex().IsMatch(uriPath)
            && token.TokenType == TokenType.GameUnAuthorized)
        {
            return token;
        }
        if ((uriPath.StartsWith(GameEndpointAttribute.BaseRoute) 
             || uriPath.StartsWith("/identity/"))
            && token.TokenType == TokenType.GameAccess)
        { 
            return token;
        }
        
        if (uriPath.StartsWith(ApiEndpointAttribute.BaseRoute) && token.TokenType == TokenType.ApiAccess)
        {
            return token;
        }

        return null;
    }
}
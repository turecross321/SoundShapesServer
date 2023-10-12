using Bunkum.Listener.Request;
using Bunkum.Core.Authentication;
using Bunkum.Core.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.TokenHelper;

namespace SoundShapesServer.Authentication;

public class AuthenticationProvider : IAuthenticationProvider<AuthToken>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        GameUser? user = AuthenticateToken(request, db)?.User;
        if (user == null) return null;

        user.RateLimitUserId = user.Id;
        return user;
    }

    public AuthToken? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        // check the session header. this is what the game uses
        string? tokenId = request.RequestHeaders["X-OTG-Identity-SessionId"];

        // check the authorization header. this is what the api uses.
        tokenId ??= request.RequestHeaders["Authorization"];

        // we dont have a token if null, so bail 
        if (tokenId == null) return null;

        GameDatabaseContext database = (GameDatabaseContext)db.Value;

        AuthToken? token = database.GetTokenWithId(tokenId);
        if (token == null) return null;

        if (token.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveToken(token);
            return null;
        }
        
        if (!IsTokenAllowedToAccessEndpoint(token, request.Uri.AbsolutePath)) return null;
        return token;
    }
}
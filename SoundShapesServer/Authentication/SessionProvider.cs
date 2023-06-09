using System.Diagnostics;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Authentication;

public class SessionProvider : IAuthenticationProvider<GameUser, GameSession>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        GameUser? user = AuthenticateToken(request, db)?.User;
        if (user == null) return null;

        user.RateLimitUserId = user.Id;
        return user;
    }

    public GameSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? sessionId;

        // check the session header. this is what the game uses
        sessionId = request.RequestHeaders["X-OTG-Identity-SessionId"];
        
        // check the authorization header. this is what the api uses.
        if (sessionId == null)
        {
            sessionId = request.RequestHeaders["Authorization"];
        }

        // we dont have a session if null, so bail 
        if (sessionId == null) return null;

        GameDatabaseContext database = (GameDatabaseContext)db.Value;
        Debug.Assert(database != null);

        GameSession? session = database.GetSessionWithId(sessionId);
        if (session == null) return null;

        if (session.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveSession(session);
            return null;
        }
        
        if (!IsSessionAllowedToAccessEndpoint(session, request.Uri.AbsolutePath)) return null;
        return session;
    }
}
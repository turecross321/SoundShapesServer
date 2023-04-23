using System.Diagnostics;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Authentication;

public class SessionProvider : IAuthenticationProvider<GameUser, GameSession>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db) 
        => this.AuthenticateToken(request, db)?.User;

    public GameSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? sessionId = null;

        // check the session header. this is what the game uses
        sessionId = request.RequestHeaders["X-OTG-Identity-SessionId"];
        
        // check the authorization header. this is what the api uses.
        if (sessionId == null)
        {
            sessionId = request.RequestHeaders["Authorization"];
        }

        // we dont have a session if null, so bail 
        if (sessionId == null) return null;

        RealmDatabaseContext database = (RealmDatabaseContext)db.Value;
        Debug.Assert(database != null);

        GameSession? session = database.GetSessionWithSessionId(sessionId);
        if (session == null) return null;

        string uriPath = request.Uri.AbsolutePath;

        if (IsSessionAllowedToAccessEndpoint(session, uriPath)) return session;

        return null;
    }
}
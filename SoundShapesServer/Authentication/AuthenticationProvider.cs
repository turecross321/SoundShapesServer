using System.Collections.Specialized;
using System.Diagnostics;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class AuthenticationProvider : IAuthenticationProvider<GameUser, Session>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db) 
        => this.AuthenticateToken(request, db)?.User;

    public Session? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? sessionId = null;

        // get the request headers
        NameObjectCollectionBase.KeysCollection keys = request.RequestHeaders.Keys;

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

        return database.GetSessionWithSessionId(sessionId);
    }
}
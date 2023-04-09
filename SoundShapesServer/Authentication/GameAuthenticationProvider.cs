using System.Collections.Specialized;
using System.Diagnostics;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameAuthenticationProvider : IAuthenticationProvider<GameUser, GameSession>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db) 
        => this.AuthenticateToken(request, db)?.User;

    public GameSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? sessionId = null;

        // get the request headers
        NameObjectCollectionBase.KeysCollection keys = request.RequestHeaders.Keys;

        // check if there is a session header
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == "X-OTG-Identity-SessionId")
                sessionId = request.RequestHeaders.GetValues(i)[0];
        }

        // we dont have a session if null, so bail 
        if (sessionId == null) return null;

        RealmDatabaseContext database = (RealmDatabaseContext)db.Value;
        Debug.Assert(database != null);

        return database.GetSessionWithSessionId(sessionId);
    }
}
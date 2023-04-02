using System.Diagnostics;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameAuthenticationProvider : IAuthenticationProvider<GameUser>
{
    public GameUser? AuthenticateUser(ListenerContext request, IDatabaseContext db)
    {
        RealmDatabaseContext database = (RealmDatabaseContext)db;
        Debug.Assert(database != null);

        string? sessionId = null;

        // get the request headers
        var keys = request.RequestHeaders.Keys;

        // check if there is a session header
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == "X-OTG-Identity-SessionId")
                sessionId = request.RequestHeaders.GetValues(i)[0];
        }

        // we dont have a session if null, so bail 
        if (sessionId == null) return null;

        return database.GetUserWithSessionId(sessionId);
    }
}
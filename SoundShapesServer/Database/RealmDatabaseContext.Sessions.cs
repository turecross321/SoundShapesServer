using MongoDB.Bson.IO;
using NPTicket;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Enums;
using SoundShapesServer.Types;
using Session = Realms.Sync.Session;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day

    public GameSession GenerateSessionForUser(GameUser user, string platform, int? tokenExpirySeconds = null)
    {
        GameSession gameSession = new()
        {
            expires = DateTimeOffset.Now.AddSeconds(tokenExpirySeconds ?? DefaultTokenExpirySeconds).ToUnixTimeSeconds(),
            id = GenerateGuid(),
            user = user,
            platform = platform
        };

        IQueryable<GameSession>? previousSessions = this._realm.All<GameSession>().Where(s => s.user == user);
        
        this._realm.Write(() =>
        {
            this._realm.RemoveRange(previousSessions.Where(s=>s.platform == platform)); // removes all previous sessions with the same platform
            this._realm.Add(gameSession);
        });

        return gameSession;
    }

    public GameUser? GetUserWithSessionId(string sessionId)
    {
        this._realm.Refresh();
        
        var sessions = this._realm.All<GameSession>();
        GameSession? session = this._realm.All<GameSession>()
            .FirstOrDefault(s => s.id == sessionId);

        if (session == null)
        {
            return null;
        }

        if (session.expires < DateTimeOffset.Now.ToUnixTimeSeconds())
        {
            this._realm.Write(() => this._realm.Remove(session));
            return null;
        }

        return session.user;
    }
}
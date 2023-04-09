using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day

    public GameSession GenerateSessionForUser(GameUser user, string platform, int? tokenExpirySeconds = null)
    {
        GameSession gameSession = new()
        {
            Expires = DateTimeOffset.UtcNow.AddSeconds(tokenExpirySeconds ?? DefaultTokenExpirySeconds),
            Id = GenerateGuid(),
            User = user,
            Platform = platform
        };

        IQueryable<GameSession>? previousSessions = this._realm.All<GameSession>().Where(s => s.User == user);
        
        this._realm.Write(() =>
        {
            this._realm.RemoveRange(previousSessions.Where(s=>s.Platform == platform)); // removes all previous sessions with the same platform
            this._realm.Add(gameSession);
        });

        return gameSession;
    }

    public GameSession? GetSessionWithSessionId(string sessionId)
    {
        this._realm.Refresh();
        
        IQueryable<GameSession>? sessions = this._realm.All<GameSession>();
        GameSession? session = this._realm.All<GameSession>()
            .FirstOrDefault(s => s.Id == sessionId);

        if (session == null)
        {
            return null;
        }

        if (session.Expires < DateTimeOffset.UtcNow)
        {
            this._realm.Write(() => this._realm.Remove(session));
            return null;
        }

        return session;
    }

    public bool IsSessionInvalid(string id)
    {
        return (this._realm.All<GameSession>().FirstOrDefault(s => s.Id == id) == null);
    }
}
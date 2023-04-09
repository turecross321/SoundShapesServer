using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day

    public Session GenerateSessionForUser(GameUser user, PlatformType platform, int tokenExpirySeconds = DefaultTokenExpirySeconds)
    {
        string platformString = platform.ToString();
        
        Session session = new()
        {
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenExpirySeconds),
            Id = GenerateGuid(),
            User = user,
            Platform = platformString
        };

        IQueryable<Session>? previousSessions = this._realm.All<Session>().Where(s => s.User == user);
        
        this._realm.Write(() =>
        {
            this._realm.RemoveRange(previousSessions.Where(s=>s.Platform == platformString)); // removes all previous sessions with the same platform
            this._realm.Add(session);
        });

        return session;
    }

    public Session? GetSessionWithSessionId(string sessionId)
    {
        this._realm.Refresh();
        
        IQueryable<Session>? sessions = this._realm.All<Session>();
        Session? session = this._realm.All<Session>()
            .FirstOrDefault(s => s.Id == sessionId);

        if (session == null)
        {
            return null;
        }

        if (session.ExpiresAt < DateTimeOffset.UtcNow)
        {
            this._realm.Write(() => this._realm.Remove(session));
            return null;
        }

        return session;
    }

    public bool IsSessionInvalid(string id)
    {
        return (this._realm.All<Session>().FirstOrDefault(s => s.Id == id) == null);
    }
}
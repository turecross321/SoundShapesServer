using Bunkum.HttpServer;
using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day

    public GameSession GenerateSessionForUser(RequestContext context, GameUser user, int sessionPlatformType, int? expirationSeconds = null, string? id = null)
    {
        IpAuthorization ip = IpHelper.GetIpAuthorizationFromRequestContext(context, this, user);

        double sessionExpirationSeconds = expirationSeconds ?? DefaultTokenExpirySeconds;
        id ??= GenerateGuid();
        
        GameSession session = new()
        {
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds),
            Id = id,
            User = user,
            SessionType = sessionPlatformType,
            Ip = ip
        };

        IQueryable<GameSession> previousSessions = this._realm.All<GameSession>().Where(s => s.User == user);
        
        this._realm.Write(() =>
        {
            this._realm.RemoveRange(previousSessions.Where(s=>s.SessionType == sessionPlatformType)); // removes all previous sessions with the same platform
            this._realm.Add(session);
        });

        return session;
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

        if (session.ExpiresAt < DateTimeOffset.UtcNow)
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

    public GameSession[] GetSessionsWithIp(IpAuthorization ip)
    {
        return this._realm.All<GameSession>().Where(s => s.Ip == ip).ToArray();
    }
}
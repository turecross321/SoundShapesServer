using Bunkum.HttpServer;
using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day
    private const int SessionLimit = 3; // limit of how many sessions with one TypeOfSession you can have

    public GameSession GenerateSessionForUser(RequestContext context, GameUser user, SessionType sessionType, int? expirationSeconds = null, string? id = null)
    {
        IpAuthorization ip = IpHelper.GetIpAuthorizationFromRequestContext(context, this, user, sessionType);

        double sessionExpirationSeconds = expirationSeconds ?? DefaultTokenExpirySeconds;
        id ??= GenerateGuid();
        
        GameSession session = new()
        {
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds),
            Id = id,
            User = user,
            SessionType = (int)sessionType,
            Ip = ip
        };

        IEnumerable<GameSession> sessionsToDelete = this._realm.All<GameSession>()
            .Where(s => s.User == user && s.SessionType == (int)sessionType)
            .AsEnumerable()
            .TakeLast(SessionLimit - 1);

        this._realm.Write(() =>
        {
            foreach (GameSession gameSession in sessionsToDelete)
            {
                this._realm.Remove(gameSession);
            }
            
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
        return this._realm.All<GameSession>().FirstOrDefault(s => s.Id == id) == null;
    }

    public GameSession[] GetSessionsWithIp(IpAuthorization ip)
    {
        return this._realm.All<GameSession>().Where(s => s.Ip == ip).ToArray();
    }

    public void RemoveSession(GameSession session)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(session);
        });
    }
}
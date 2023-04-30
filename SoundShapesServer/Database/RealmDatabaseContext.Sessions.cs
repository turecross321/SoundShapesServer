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
            Id = id,
            User = user,
            SessionType = (int)sessionType,
            Ip = ip,
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds)
        };

        IEnumerable<GameSession> sessionsToDelete = _realm.All<GameSession>()
            .AsEnumerable()
            .Where(s => s.User?.Id == user.Id && s.SessionType == (int)sessionType)
            .TakeLast(SessionLimit - 1);

        _realm.Write(() =>
        {
            foreach (GameSession gameSession in sessionsToDelete)
            {
                _realm.Remove(gameSession);
            }
            
            _realm.Add(session);
        });

        return session;
    }

    public GameSession? GetSessionWithSessionId(string sessionId)
    {
        _realm.Refresh();
        
        _realm.All<GameSession>();
        GameSession? session = _realm.All<GameSession>()
            .AsEnumerable()
            .FirstOrDefault(s => s.Id == sessionId);
        
        return session;
    }

    public bool IsSessionInvalid(string id)
    {
        return _realm.All<GameSession>().FirstOrDefault(s => s.Id == id) == null;
    }

    public GameSession[] GetSessionsWithIp(IpAuthorization ip)
    {
        return _realm.All<GameSession>().Where(s => s.Ip == ip).ToArray();
    }

    public void RemoveSession(GameSession session)
    {
        _realm.Write(() =>
        {
            _realm.Remove(session);
        });
    }

    public void RemoveAllSessionsWithUser(GameUser user)
    {
        GameSession[] sessions = user.Sessions.ToArray();
        for (int i = 0; i < sessions.Length; i++)
        {
            RemoveSession(sessions[i]);
        }
    }
}
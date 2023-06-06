using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public const int DefaultSessionExpirySeconds = 86400; // 1 day
    private const int SessionLimit = 3; // limit of how many sessions of one SessionType you can have

    public GameSession CreateSession(GameUser user, SessionType sessionType, PlatformType platformType, int? expirationSeconds = null, string? id = null, IpAuthorization? ip = null)
    {
        double sessionExpirationSeconds = expirationSeconds ?? DefaultSessionExpirySeconds;
        id ??= GenerateGuid();
        
        GameSession session = new()
        {
            Id = id,
            User = user,
            SessionType = sessionType,
            PlatformType = platformType,
            Ip = ip,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds),
            CreationDate = DateTimeOffset.UtcNow
        };

        IEnumerable<GameSession> sessionsToDelete = _realm.All<GameSession>()
            .Where(s => s.User.Id == user.Id && s._SessionType == (int)sessionType)
            .AsEnumerable()
            .TakeLast(SessionLimit - 1);

        _realm.Write(() =>
        {
            foreach (GameSession gameSession in sessionsToDelete)
            {
                _realm.Remove(gameSession);
            }
            
            _realm.Add(session);
            if (sessionType == SessionType.Game) 
                user.LastGameLogin = DateTimeOffset.UtcNow;
        });

        _realm.Refresh();
        
        return session;
    }

    private GameSession[] GetSessionsWithIp(IpAuthorization ip)
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

    private void RemoveAllSessionsWithUser(GameUser user)
    {
        GameSession[] sessions = user.Sessions.ToArray();
        foreach (GameSession session in sessions)
        {
            RemoveSession(session);
        }
    }
    
    public GameSession? GetSessionWithSessionId(string sessionId)
    {
        _realm.All<GameSession>();
        GameSession? session = _realm.All<GameSession>()
            .FirstOrDefault(s => s.Id == sessionId);
        
        return session;
    }
}
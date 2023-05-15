using Bunkum.HttpServer;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private const int DefaultTokenExpirySeconds = 86400; // 1 day
    private const int SessionLimit = 3; // limit of how many sessions with of one SessionType you can have

    public GameSession CreateSession(RequestContext context, GameUser user, SessionType sessionType, int? expirationSeconds = null, string? id = null, PlatformType? platformType = null)
    {
        IpAuthorization ip = IpHelper.GetIpAuthorizationFromRequestContext(context, this, user, sessionType);

        double sessionExpirationSeconds = expirationSeconds ?? DefaultTokenExpirySeconds;
        id ??= GenerateGuid();
        
        GameSession session = new()
        {
            Id = id,
            User = user,
            SessionType = (int)sessionType,
            PlatformType = (int?)platformType,
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

        _realm.Refresh();
        
        return session;
    }

    public GameSession? GetSessionWithSessionId(string sessionId)
    {
        _realm.All<GameSession>();
        GameSession? session = _realm.All<GameSession>()
            .AsEnumerable()
            .FirstOrDefault(s => s.Id == sessionId);
        
        return session;
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

    private void RemoveAllSessionsWithUser(GameUser user)
    {
        GameSession[] sessions = user.Sessions.ToArray();
        for (int i = 0; i < sessions.Length; i++)
        {
            RemoveSession(sessions[i]);
        }
    }
}
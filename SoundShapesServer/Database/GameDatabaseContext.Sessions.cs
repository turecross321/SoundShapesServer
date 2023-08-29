using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public const int DefaultSessionExpirySeconds = Globals.OneDayInSeconds;
    private const int SimultaneousSessionsLimit = 3;

    public GameSession CreateSession(GameUser user, SessionType sessionType, PlatformType platformType, bool? genuineTicket = null, int? expirationSeconds = null, string? id = null)
    {
        double sessionExpirationSeconds = expirationSeconds ?? DefaultSessionExpirySeconds;
        id ??= GenerateGuid();
        
        GameSession session = new()
        {
            Id = id,
            User = user,
            SessionType = sessionType,
            PlatformType = platformType,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds),
            CreationDate = DateTimeOffset.UtcNow,
            GenuineNpTicket = genuineTicket
        };

        IEnumerable<GameSession> sessionsToDelete = _realm.All<GameSession>()
            .Where(s=> s.User == user && s._SessionType == (int)sessionType)
            .AsEnumerable()
            .SkipLast(SimultaneousSessionsLimit - 1);

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
    
    public GameSession? GetSessionWithId(string sessionId)
    {
        _realm.All<GameSession>();
        GameSession? session = _realm.All<GameSession>()
            .FirstOrDefault(s => s.Id == sessionId);
        
        return session;
    }
}
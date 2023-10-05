using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public const int DefaultSessionExpirySeconds = Globals.OneDayInSeconds;
    private const int SimultaneousSessionsLimit = 3;

    public GameSession CreateSession(GameUser user, SessionType sessionType, 
        double expirationSeconds = DefaultSessionExpirySeconds, PlatformType platformType = PlatformType.Unknown, 
        bool? genuineTicket = null, GameSession? refreshSession = null)
    {
        string id = sessionType switch
        {
            SessionType.SetPassword => GeneratePasswordSessionId(this),
            SessionType.SetEmail => GenerateEmailSessionId(this),
            SessionType.AccountRemoval => GenerateAccountRemovalSessionId(this),
            _ => GenerateGuid()
        };
        
        GameSession session = new()
        {
            Id = id,
            User = user,
            SessionType = sessionType,
            PlatformType = platformType,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(expirationSeconds),
            CreationDate = DateTimeOffset.UtcNow,
            GenuineNpTicket = genuineTicket,
            RefreshSession = refreshSession
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

    public void RefreshSession(GameSession session, long expirySecondsFromNow)
    {
        _realm.Write(() =>
        {
            session.ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(expirySecondsFromNow);
        });
    }

    public void RemoveSession(GameSession session)
    {
        _realm.Write(() =>
        {
            _realm.Remove(session);
        });
    }

    public void RemoveSessions(IQueryable<GameSession> sessions)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(sessions);
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
        GameSession? session = _realm.All<GameSession>()
            .FirstOrDefault(s => s.Id == sessionId);
        
        return session;
    }
}
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string username)
    {
        GameUser user = new()
        {
            Id = GenerateGuid(),
            Username = username
        };

        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
    }

    public void SetUserEmail(GameUser user, string email, GameSession session)
    {
        _realm.Write(() =>
        {
            _realm.Remove(session);
            user.Email = email;
        });
    }
    
    public void SetUserPassword(GameUser user, string password, GameSession? session = null)
    {
        _realm.Write(() =>
        {
            if (session != null) _realm.Remove(session);
            user.PasswordBcrypt = password;
            user.HasFinishedRegistration = true;
        });
    }

    public void SetUsername(GameUser user, string username)
    {
        RemoveAllSessionsWithUser(user);
        
        _realm.Write(() =>
        {
            user.Username = username;
        });
    }

    public GameUser? GetUserWithUsername(string username)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => u.Username == username);
    }

    public GameUser? GetUserWithEmail(string email)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => u.Email == email);
    }

    public GameUser? GetUserWithId(string? id)
    {
        if (id == null) return null;
        return _realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }

    public void RemoveUser(GameUser user)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Sessions);
            _realm.RemoveRange(user.Levels);
            _realm.RemoveRange(user.LeaderboardEntries);
            _realm.RemoveRange(user.Punishments);
            _realm.RemoveRange(user.IpAddresses);
            _realm.Remove(user);
            
            _realm.Remove(user);
        });
    }
}
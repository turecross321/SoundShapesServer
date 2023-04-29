using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string username)
    {
        GameUser user = new()
        {
            Username = username
        };

        this._realm.Write(() =>
        {
            this._realm.Add(user);
        });

        return user;
    }

    public void SetUserEmail(GameUser user, string email, GameSession session)
    {
        this._realm.Write(() =>
        {
            this._realm.Remove(session);
            user.Email = email;
        });
    }
    
    public void SetUserPassword(GameUser user, string password, GameSession? session = null)
    {
        this._realm.Write((() =>
        {
            if (session != null) this._realm.Remove(session);
            user.PasswordBcrypt = password;
            user.HasFinishedRegistration = true;
        }));
    }

    public GameUser? GetUserWithUsername(string username)
    {
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Username == username);
    }

    public GameUser? GetUserWithEmail(string email)
    {
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Email == email);
    }

    public GameUser? GetUserWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }
    
}
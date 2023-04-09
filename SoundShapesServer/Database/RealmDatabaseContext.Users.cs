using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string username, string password)
    {
        GameUser user = new()
        {
            Username = username,
            PasswordBcrypt = password
        };

        this._realm.Write(() =>
        {
            this._realm.Add(user);
        });

        return user;
    }
    
    public GameUser? GetUserWithUsername(string? username)
    {
        if (username == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Username == username);
    }
    
    public GameUser? GetUserWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }

    public void SetUserPassword(GameUser user, string password)
    {
        this._realm.Write((() =>
        {
            user.PasswordBcrypt = password;
        }));
    }
}
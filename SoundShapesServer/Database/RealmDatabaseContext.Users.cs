using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string displayName)
    {
        GameUser user = new()
        {
            DisplayName = displayName,
        };

        this._realm.Write(() =>
        {
            this._realm.Add(user);
        });

        return user;
    }
    
    public GameUser? GetUserWithDisplayName(string? displayName)
    {
        if (displayName == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.DisplayName == displayName);
    }
    
    public GameUser? GetUserWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }
}
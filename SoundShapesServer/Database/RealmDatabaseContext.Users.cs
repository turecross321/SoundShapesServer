using SoundShapesServer.Types;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using MongoDB.Bson;
using SoundShapesServer.Enums;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string display_name)
    {
        GameUser user = new()
        {
            display_name = display_name
        };

        this._realm.Write(() =>
        {
            this._realm.Add(user);
        });

        return user;
    }
    
    public GameUser? GetUserWithDisplayName(string? display_name)
    {
        if (display_name == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.display_name == display_name);
    }
    
    public GameUser? GetUserWithId(string? id)
    {
        if (id == null) return null;
        return this._realm.All<GameUser>().FirstOrDefault(u => u.id == id);
    }

    public bool UploadFriends(string friends, GameUser user)
    {
        this._realm.Write(() =>
        {
            user.friends = friends;
        });
        return true;
    }
    
    public string FormatUserId(string id)
    {
        return $"/~{ItemType.identity.ToString()}:{id}";
    }
}
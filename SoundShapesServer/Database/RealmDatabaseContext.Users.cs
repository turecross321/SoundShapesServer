using Bunkum.HttpServer.Storage;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public GameUser CreateUser(string username)
    {
        GameUser user = new ()
        {
            Id = GenerateGuid(),
            Username = username,
            CreationDate = DateTimeOffset.UtcNow
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

        _realm.Refresh();
    }

    public void SetUsername(GameUser user, string username)
    {
        RemoveAllSessionsWithUser(user);
        
        _realm.Write(() =>
        {
            user.Username = username;
        });
    }

    public IQueryable<GameUser> GetUsers()
    {
        return _realm.All<GameUser>();
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
        return id == null ? null : _realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }
    
    public IQueryable<GameUser> SearchForUsers(string query)
    {
        string[] keywords = query.Split(' ');
        if (keywords.Length == 0) return Enumerable.Empty<GameUser>().AsQueryable();
        
        IQueryable<GameUser>? entries = _realm.All<GameUser>();
        
        foreach (string keyword in keywords)
        {
            if (string.IsNullOrWhiteSpace(keyword)) continue;

            entries = entries.Where(u =>
                u.Username.Like(keyword, false)
            );
        }

        return entries;
    }

    public void RemoveUser(GameUser user, IDataStore dataStore)
    {
        foreach (GameLevel level in user.Levels)
        {
            RemoveLevel(level, dataStore);
        }
        
        dataStore.RemoveFromStore(GetSaveResourceKey(user.Id));
        
        RemoveAllReportsWithContentId(user.Id);
        
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Sessions);
            _realm.RemoveRange(user.IpAddresses);
            _realm.RemoveRange(user.Followers);
            _realm.RemoveRange(user.Following);

            string id = user.Id;
            string username = user.Username;
            
            _realm.Remove(user);
            
            _realm.Add(new GameUser
            {
                Id = id,
                Username = username,
                Deleted = true
            });
        });
    }

    public void SetUserPermissions(GameUser user, PermissionsType type)
    {
        _realm.Write(() =>
        {
            user.PermissionsType = (int)type;
        });
    }
}
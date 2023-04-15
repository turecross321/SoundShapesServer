using SoundShapesServer.Authentication;
using SoundShapesServer.Requests.Api;
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

    public void AddUnAuthenticatedIpAddress(GameUser user, string ipAddress)
    {
        // Check if ip address has already been tracked
        if (this._realm.All<IpAuthenticationRequest>()
                .FirstOrDefault(r => r.User == user && r.IpAddress == ipAddress) != null)
            return;
        
        this._realm.Write(() =>
        {
            this._realm.Add(new IpAuthenticationRequest()
            {
                IpAddress = ipAddress,
                User = user
            });
        });
    }
    public bool AddAuthenticatedIpAddress(GameUser user, string ipAddress)
    {
        if (user.AuthorizedIPAddresses.Contains(ipAddress)) return false;
        
        this._realm.Write(() =>
        {
            user.AuthorizedIPAddresses.Add(ipAddress);
        });

        return true;
    }

    public bool RemoveAuthenticatedIpAddress(GameUser user, string ipAddress)
    {
        if (user.AuthorizedIPAddresses.Contains(ipAddress) == false) return false;
        
        this._realm.Write(() =>
        {
            user.AuthorizedIPAddresses.Remove(ipAddress);
        });

        return true;
    }
}
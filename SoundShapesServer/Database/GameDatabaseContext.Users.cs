using Bunkum.Core.Storage;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private const string AdminUsername = "admin";
    public const string AdminId = "00000000-0000-0000-0000-000000000000";
    public GameUser GetAdminUser()
    {
        GameUser? user = GetUserWithId(AdminId);
        if (user != null) return user;

        user ??= new GameUser
        {
            Id = AdminId,
            Username = AdminUsername,
            PermissionsType = PermissionsType.Administrator,
            HasFinishedRegistration = true
        };
        
        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
    }

    public void RemoveUser(GameUser user, IDataStore dataStore)
    {
        foreach (GameLevel level in user.Levels)
        {
            RemoveLevel(level, dataStore);
        }

        if (user.SaveFilePath != null) 
            dataStore.RemoveFromStore(user.SaveFilePath);

        RemoveAllReportsWithContentUser(user);
        
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Tokens);
            _realm.RemoveRange(user.IpAddresses);
            _realm.RemoveRange(user.FollowersRelations);
            _realm.RemoveRange(user.FollowingRelations);
            _realm.RemoveRange(user.LikedLevelRelations);
            _realm.RemoveRange(user.PlayedLevelRelations);
            _realm.RemoveRange(user.QueuedLevelRelations);
            _realm.RemoveRange(user.Events);
            RemoveEventsOnUser(user);
            _realm.RemoveRange(user.LeaderboardEntries);
            _realm.RemoveRange(user.NewsEntries);

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

    public void SetUserPermissions(GameUser user, PermissionsType permissionsType)
    {
        _realm.Write(() =>
        {
            user.PermissionsType = permissionsType;
        });
    }
    
    public GameUser CreateUser(string username)
    {
        GameUser user = new()
        {
            Id = GenerateGuid(),
            Username = username,
            CreationDate = DateTimeOffset.UtcNow,
        };

        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
    }

    private const int WorkFactor = 10;
    public bool ValidatePassword(GameUser user, string hash)
    {
        if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
        {
            SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor));
        }

        return BCrypt.Net.BCrypt.Verify(hash.ToLower(), user.PasswordBcrypt);
    }
    public void SetUserPassword(GameUser user, string hash)
    {
        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor);
        
        _realm.Write(() =>
        {
            user.PasswordBcrypt = passwordBcrypt;
        });

        RemoveUserTokens(user);

        _realm.Refresh();
    }

    public void FinishRegistration(GameUser user)
    {
        _realm.Write(() =>
        {
            user.HasFinishedRegistration = true;
        });
        
        CreateEvent(user, EventType.AccountRegistration, PlatformType.Unknown, EventDataType.User, user.Id);
    }
    
    public void SetUserEmail(GameUser user, string email)
    {
        _realm.Write(() =>
        {
            user.Email = email;
        });
    }

    public void SetUsername(GameUser user, string username)
    {
        _realm.Write(() =>
        {
            user.Username = username;
        });
    }

    private void SetUserPlayTime(GameUser user)
    {
        _realm.Write(() =>
        {
            user.TotalPlayTime = user.LeaderboardEntries.AsEnumerable().Sum(e => e.PlayTime);
        });
    }

    public void SetUserSaveFilePath(GameUser user, string? path)
    {
        _realm.Write(() =>
        {
            user.SaveFilePath = path;
        });
    }

    public void SetUserGameAuthenticationSettings(GameUser user, ApiSetGameAuthenticationSettingsRequest request)
    {
        if (!request.AllowPsnAuthentication)
            RemoveUserTokensOfAuthenticationType(user, TokenAuthenticationType.Psn);
        if (!request.AllowRpcnAuthentication)
            RemoveUserTokensOfAuthenticationType(user, TokenAuthenticationType.Rpcn);
        if (!request.AllowIpAuthentication)
        {
            RemoveIpAddresses(user.IpAddresses);
            RemoveUserTokensOfAuthenticationType(user, TokenAuthenticationType.Ip);
        }
        
        _realm.Write(() =>
        {
            user.AllowPsnAuthentication = request.AllowPsnAuthentication;
            user.AllowRpcnAuthentication = request.AllowRpcnAuthentication;
            user.AllowIpAuthentication = request.AllowIpAuthentication;
        });
    }
    
    public GameUser? GetUserWithUsername(string username, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => 
            (!u.Deleted || u.Deleted == includeDeleted) 
            && u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public GameUser? GetUserWithEmail(string email, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => 
            (!u.Deleted || u.Deleted == includeDeleted) 
            && u.Email != null 
            && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public GameUser? GetUserWithId(string id, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => (!u.Deleted || u.Deleted == includeDeleted) && u.Id == id);
    }

    public (GameUser[], int) GetPaginatedUsers(UserOrderType order, bool descending, UserFilters filters, int from, int count)
    {
        IQueryable<GameUser> users = _realm.All<GameUser>().FilterUsers(filters).OrderUsers(order, descending);
        return (users.Paginate(from, count), users.Count());
    }
}
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameUser CreateUser(string username)
    {
        GameUser user = new ()
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
            SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(hash, WorkFactor));
        }

        return BCrypt.Net.BCrypt.Verify(hash.ToLower(), user.PasswordBcrypt);
    }
    public bool SetUserPassword(GameUser user, string hash)
    {
        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor);
        
        _realm.Write(() =>
        {
            user.PasswordBcrypt = passwordBcrypt;
            user.HasFinishedRegistration = true;
        });
        
        // Only create the event when the user has finished registration and can actually connect.
        CreateEvent(user, EventType.AccountRegistration, user);

        _realm.Refresh();
        return true;
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
        RemoveAllSessionsWithUser(user);
        
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

    public (GameUser[], int) GetUsers(UserOrderType order, bool descending, UserFilters filters, int from, int count)
    {
        IQueryable<GameUser> orderedUsers = order switch
        {
            UserOrderType.FollowersCount => UsersOrderedByFollowersCount(descending),
            UserOrderType.FollowingCount => UsersOrderedByFollowingCount(descending),
            UserOrderType.LevelsCount => UsersOrderedByLevelsCount(descending),
            UserOrderType.LikedLevelsCount => UsersOrderedByLikedLevelsCount(descending),
            UserOrderType.CreationDate => UsersOrderedByCreationDate(descending),
            UserOrderType.PlayedLevelsCount => UsersOrderedByPlayedLevelsCount(descending),
            UserOrderType.CompletedLevelsCount => UsersOrderedByCompletedLevelsCount(descending),
            UserOrderType.Deaths => UsersOrderedByDeaths(descending),
            UserOrderType.TotalPlayTime => UsersOrderedByTotalPlayTime(descending),
            _ => UsersOrderedByCreationDate(descending)
        };

        IQueryable<GameUser> filteredUsers = FilterUsers(orderedUsers, filters);
        GameUser[] paginatedUsers = PaginateUsers(filteredUsers, from, count);

        return (paginatedUsers, filteredUsers.Count());
    }
    
    private IQueryable<GameUser> FilterUsers(IQueryable<GameUser> users, UserFilters filters)
    {
        IQueryable<GameUser> response = users;
        
        if (filters.IsFollowingUser != null)
        {
            IQueryable<FollowRelation> relations = filters.IsFollowingUser.Followers;

            List<GameUser> tempResponse = new ();

            foreach (FollowRelation relation in relations)
            {
                GameUser follower = relation.Follower;
                GameUser? responseUser = response.FirstOrDefault(u => u.Id == follower.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.FollowedByUser != null)
        {
            IQueryable<FollowRelation> relations = filters.FollowedByUser.Following;

            List<GameUser> tempResponse = new ();

            foreach (FollowRelation relation in relations)
            {
                GameUser recipient = relation.Recipient;
                GameUser? responseUser = response.FirstOrDefault(u => u.Id == recipient.Id);
                if (responseUser != null) tempResponse.Add(responseUser);
            }

            response = tempResponse.AsQueryable();
        }
        
        if (filters.Search != null)
        {
            response = response.Where(l => l.Username.Contains(filters.Search, StringComparison.OrdinalIgnoreCase));
        }

        return response;
    }

    private IQueryable<GameUser> UsersOrderedByFollowersCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.FollowersCount);
        return _realm.All<GameUser>().OrderBy(u => u.FollowersCount);
    }
    
    private IQueryable<GameUser> UsersOrderedByFollowingCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.FollowingCount);
        return _realm.All<GameUser>().OrderBy(u => u.FollowingCount);
    } 
    
    private IQueryable<GameUser> UsersOrderedByLevelsCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.LevelsCount);
        return _realm.All<GameUser>().OrderBy(u => u.LevelsCount);
    } 
    
    private IQueryable<GameUser> UsersOrderedByLikedLevelsCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.LikedLevelsCount);
        return _realm.All<GameUser>().OrderBy(u => u.LikedLevelsCount);
    } 
    
    private IQueryable<GameUser> UsersOrderedByCreationDate(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.CreationDate);
        return _realm.All<GameUser>().OrderBy(u => u.CreationDate);
    } 
    
    private IQueryable<GameUser> UsersOrderedByPlayedLevelsCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.PlayedLevelsCount);
        return _realm.All<GameUser>().OrderBy(u => u.PlayedLevelsCount);
    } 
    
    private IQueryable<GameUser> UsersOrderedByCompletedLevelsCount(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.CompletedLevelsCount);
        return _realm.All<GameUser>().OrderBy(u => u.CompletedLevelsCount);
    } 
    
    private IQueryable<GameUser> UsersOrderedByDeaths(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.Deaths);
        return _realm.All<GameUser>().OrderBy(u => u.Deaths);
    }
    
    private IQueryable<GameUser> UsersOrderedByTotalPlayTime(bool descending)
    {
        if (descending) return _realm.All<GameUser>().OrderByDescending(u => u.TotalPlayTime);
        return _realm.All<GameUser>().OrderBy(u => u.TotalPlayTime);
    }

    private const string AdminUsername = "admin";
    public GameUser GetAdminUser()
    {
        GameUser? user = GetUserWithUsername(AdminUsername);
        if (user != null) return user;

        user ??= new GameUser()
        {
            Id = new Guid().ToString(), // 00000000-0000-0000-0000-000000000000
            Username = AdminUsername,
            PermissionsType = (int)PermissionsType.Administrator,
            HasFinishedRegistration = true
        };
        
        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
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

    public void RemoveUser(GameUser user, IDataStore dataStore)
    {
        foreach (GameLevel level in user.Levels)
        {
            RemoveLevel(level, dataStore);
        }

        if (user.SaveFilePath != null) dataStore.RemoveFromStore(user.SaveFilePath);

        RemoveAllReportsWithContentUser(user);
        
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Sessions);
            _realm.RemoveRange(user.IpAddresses);
            _realm.RemoveRange(user.Followers);
            _realm.RemoveRange(user.Following);
            _realm.RemoveRange(user.Events);
            _realm.RemoveRange(user.EventsWhereUserIsRecipient);

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
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private const string AdminUsername = "admin";
    public GameUser GetAdminUser()
    {
        GameUser? user = GetUserWithUsername(AdminUsername);
        if (user != null) return user;

        user ??= new GameUser()
        {
            Id = new Guid().ToString(), // 00000000-0000-0000-0000-000000000000
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
            user.HasFinishedRegistration = true;
        });
        
        // Only create the event when the user has finished registration and can actually connect.
        CreateEvent(user, EventType.AccountRegistration, user);

        _realm.Refresh();
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
    
    public void SetFeaturedLevel(GameUser user, GameLevel level)
    {
        _realm.Write(() =>
        {
            user.FeaturedLevel = level;
        });
    }
    
    public GameUser? GetUserWithUsername(string username, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u =>  (!u.Deleted || u.Deleted == includeDeleted) && u.Username == username);
    }

    public GameUser? GetUserWithEmail(string email, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u =>  (!u.Deleted || u.Deleted == includeDeleted) && u.Email == email);
    }

    public GameUser? GetUserWithId(string id, bool includeDeleted = false)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => (!u.Deleted || u.Deleted == includeDeleted) && u.Id == id);
    }

    public (GameUser[], int) GetUsers(UserOrderType order, bool descending, UserFilters filters, int from, int count, 
        bool includeDeleted = false)
    {
        // Including Deleted is not a filter because it should only be accessible in internal server stuff
        IQueryable<GameUser> users = _realm.All<GameUser>().Where(u=>!u.Deleted || u.Deleted == includeDeleted);

        IQueryable<GameUser> filteredUsers = FilterUsers(users, filters);
        IQueryable<GameUser> orderedUsers = OrderUsers(filteredUsers, order, descending);
        
        GameUser[] paginatedUsers = PaginateUsers(orderedUsers, from, count);

        return (paginatedUsers, filteredUsers.Count());
    }
    
    private static IQueryable<GameUser> FilterUsers(IQueryable<GameUser> users, UserFilters filters)
    {
        IQueryable<GameUser> response = users;
        
        if (filters.IsFollowingUser != null)
        {
            IQueryable<FollowRelation> relations = filters.IsFollowingUser.Followers;

            List<GameUser> tempResponse = new();

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

            List<GameUser> tempResponse = new();

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

    #region Order Users

    private static IQueryable<GameUser> OrderUsers(IQueryable<GameUser> users, UserOrderType order, bool descending)
    {
        return order switch
        {
            UserOrderType.Followers => OrderUsersByFollowers(users, descending),
            UserOrderType.Following => OrderUsersByFollowing(users, descending),
            UserOrderType.PublishedLevels => OrderUsersByLevels(users, descending),
            UserOrderType.LikedLevels => OrderUsersByLikedLevels(users, descending),
            UserOrderType.CreationDate => OrderUsersByCreationDate(users, descending),
            UserOrderType.PlayedLevels => OrderUsersByPlayedLevels(users, descending),
            UserOrderType.CompletedLevels => OrderUsersByCompletedLevels(users, descending),
            UserOrderType.Deaths => OrderUsersByDeaths(users, descending),
            UserOrderType.TotalPlayTime => OrderUsersByPlayTime(users, descending),
            UserOrderType.LastGameLogin => OrderUsersByLastGameLogin(users, descending),
            UserOrderType.Events => OrderUsersByEvents(users, descending),
            _ => users
        };
    }
    
    private static IQueryable<GameUser> OrderUsersByFollowers(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.FollowersCount);
        return users.OrderBy(u => u.FollowersCount);
    }
    
    private static IQueryable<GameUser> OrderUsersByFollowing(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.FollowingCount);
        return users.OrderBy(u => u.FollowingCount);
    } 
    
    private static IQueryable<GameUser> OrderUsersByLevels(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.LevelsCount);
        return users.OrderBy(u => u.LevelsCount);
    } 
    
    private static IQueryable<GameUser> OrderUsersByLikedLevels(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.LikedLevelsCount);
        return users.OrderBy(u => u.LikedLevelsCount);
    } 
    
    private static IQueryable<GameUser> OrderUsersByCreationDate(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.CreationDate);
        return users.OrderBy(u => u.CreationDate);
    } 
    
    private static IQueryable<GameUser> OrderUsersByPlayedLevels(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.PlayedLevelsCount);
        return users.OrderBy(u => u.PlayedLevelsCount);
    } 
    
    private static IQueryable<GameUser> OrderUsersByCompletedLevels(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.CompletedLevelsCount);
        return users.OrderBy(u => u.CompletedLevelsCount);
    } 
    
    private static IQueryable<GameUser> OrderUsersByDeaths(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.Deaths);
        return users.OrderBy(u => u.Deaths);
    }
    
    private static IQueryable<GameUser> OrderUsersByPlayTime(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.TotalPlayTime);
        return users.OrderBy(u => u.TotalPlayTime);
    }
    
    private static IQueryable<GameUser> OrderUsersByLastGameLogin(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.LastGameLogin);
        return users.OrderBy(u => u.LastGameLogin);
    }
    
    private static IQueryable<GameUser> OrderUsersByEvents(IQueryable<GameUser> users, bool descending)
    {
        if (descending) return users.OrderByDescending(u => u.EventsCount);
        return users.OrderBy(u => u.EventsCount);
    }
    
    #endregion
}
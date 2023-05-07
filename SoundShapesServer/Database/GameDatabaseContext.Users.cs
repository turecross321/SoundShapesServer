using Bunkum.HttpServer.Storage;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PaginationHelper;
using static SoundShapesServer.Helpers.ResourceHelper;
using static SoundShapesServer.Helpers.UserHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameUser CreateUser(string username, bool skipRegistration = false)
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
    
    public bool SetUserPassword(GameUser user, string password, GameSession? session = null)
    {
        _realm.Write(() =>
        {
            if (session != null) _realm.Remove(session);
            user.PasswordBcrypt = password;
            user.HasFinishedRegistration = true;
        });
        
        // Only create the event when the user has finished registration and can actually connect.
        CreateEvent(user, EventType.AccountRegistration, user);

        _realm.Refresh();
        return true;
    }

    public void SetUsername(GameUser user, string username)
    {
        RemoveAllSessionsWithUser(user);
        
        _realm.Write(() =>
        {
            user.Username = username;
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
            UserOrderType.LeaderboardPlacements => UsersOrderedByLeaderboardPlacements(this, descending),
            _ => UsersOrderedByCreationDate(descending)
        };

        IQueryable<GameUser> filteredUsers = FilterUsers(orderedUsers, filters);
        GameUser[] paginatedUsers = PaginateUsers(filteredUsers, from, count);

        return (paginatedUsers, filteredUsers.Count());
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
    
    private IQueryable<GameUser> UsersOrderedByLeaderboardPlacements(GameDatabaseContext database, bool descending)
    {
        if (descending) return _realm.All<GameUser>()
            .AsEnumerable()
            .OrderByDescending(u => LeaderboardHelper.GetTotalLeaderboardPlacements(database, u))
            .AsQueryable();
        return _realm.All<GameUser>()            
            .AsEnumerable()
            .OrderBy(u => LeaderboardHelper.GetTotalLeaderboardPlacements(database, u))
            .AsQueryable();
    } 

    public GameUser GetServerUser()
    {
        GameUser? user = GetUserWithUsername("Server");
        return user ?? CreateUser("Server", true);
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
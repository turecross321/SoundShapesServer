using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private GameIp CreateIpAddress(GameUser user, string ipAddress)
    {
        GameIp gameIp = new()
        {
            IpAddress = ipAddress,
            User = user,
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow
        };
        
        _realm.Write(() =>
        {
            _realm.Add(gameIp);
        });

        return gameIp;
    }

    public GameIp GetIpFromAddress(GameUser user, string ipAddress)
    {
        _realm.Refresh();
        
        GameIp? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress);
        return ip ?? CreateIpAddress(user, ipAddress);
    }
    public bool AuthorizeIpAddress(GameIp gameIp, bool oneTime)
    {
        _realm.Write(() =>
        {
            gameIp.Authorized = true;
            gameIp.OneTimeUse = oneTime;
            gameIp.ModificationDate = DateTimeOffset.UtcNow;

            foreach (GameToken token in gameIp.Tokens.Where(s=>s._TokenType == (int)TokenType.GameUnAuthorized))
            {
                token.TokenType = TokenType.GameAccess;
            }
        });

        _realm.Refresh();
        
        return true;
    }

    public void RemoveIpAddress(GameIp gameIp)
    {
        _realm.Write(() =>
        {
            // Remove all tokens with ip address
            foreach (GameToken token in gameIp.Tokens)
            {
                _realm.Remove(token);
            }
            
            _realm.Remove(gameIp);
        });

        _realm.Refresh();
    }

    private void RemoveIpAddresses(IQueryable<GameIp> ips)
    {
        _realm.Write(() =>
        {
            foreach (GameIp gameIp in ips)
            {
                // Remove all tokens with ip address
                foreach (GameToken token in gameIp.Tokens)
                {
                    _realm.Remove(token);
                }
            
                _realm.Remove(gameIp);
            }
        });

        _realm.Refresh();
    }

    public void UseOneTimeIpAddress(GameIp gameIp)
    {
        _realm.Write(() =>
        {
            gameIp.Authorized = false;
            gameIp.OneTimeUse = false;
            gameIp.ModificationDate = DateTimeOffset.UtcNow;
        });
    }

    public (GameIp[], int) GetPaginatedIps(GameUser user, bool? authorized, int from, int count)
    {
        IQueryable<GameIp> filteredAddresses = GetIps(user, authorized);
        GameIp[] paginatedAddresses = PaginationHelper.PaginateIpAddresses(filteredAddresses, from, count);

        return (paginatedAddresses, filteredAddresses.Count());
    }

    private IQueryable<GameIp> GetIps(GameUser user, bool? authorized)
    {
        IQueryable<GameIp> addresses = _realm.All<GameIp>().Where(i => i.User == user);
        IQueryable<GameIp> filteredAddresses = FilterIpAddresses(addresses, authorized);

        return filteredAddresses;
    }

    private static IQueryable<GameIp> FilterIpAddresses(IQueryable<GameIp> addresses,
        bool? authorized)
    {
        IQueryable<GameIp> response = addresses;

        if (authorized != null)
        {
            response = response.Where(i => i.Authorized == authorized);
        }

        return response;
    }
}
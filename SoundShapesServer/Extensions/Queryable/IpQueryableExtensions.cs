using SoundShapesServer.Types;

namespace SoundShapesServer.Extensions.Queryable;

public static class IpQueryableExtensions
{
    public static IQueryable<GameIp> FilterIpAddresses(this IQueryable<GameIp> addresses,
        bool? authorized)
    {
        if (authorized != null)
        {
            addresses = addresses.Where(i => i.Authorized == authorized);
        }

        return addresses;
    }
}
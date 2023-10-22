using System.Net;
using Bunkum.Core;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions.RequestContextExtensions;

public static class RequestContextExtensions
{
    public static (int, int, bool) GetPageData(this RequestContext context, bool descendingIfNull = true)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = context.QueryString["descending"].ToBool() ?? descendingIfNull;
        
        return (from, count, descending);
    }

    public static string GetIpAddress(this RequestContext context)
    {
        return ((IPEndPoint)context.RemoteEndpoint).Address.ToString();;
    }
    public static GameIp? GetGameIp(this RequestContext context, GameDatabaseContext database, GameUser user)
    {
        string ipAddress = GetIpAddress(context);
        GameIp? gameIp = database.GetIpWithAddress(user, ipAddress);

        return gameIp;
    }
}
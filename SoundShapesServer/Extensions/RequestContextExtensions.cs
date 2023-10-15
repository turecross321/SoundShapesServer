using System.Net;
using Bunkum.Core;
using JetBrains.Annotations;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions;

public static class RequestContextExtensions
{
    public static (int, int, bool) GetPageData(this RequestContext context, bool descendingIfNull = true)
    {
        const int maxCount = 100;
        
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        if (count > maxCount) count = maxCount;

        bool descending = descendingIfNull;
        if (bool.TryParse(context.QueryString["descending"], out bool tempDescending))
            descending = tempDescending;
        
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
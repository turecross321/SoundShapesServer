using Bunkum.Core;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class PaginationHelper
{
    public static (int, int, bool) GetPageData(RequestContext context, bool descendingIfNull = true)
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
    
    public static (int?, int?) GetPageTokens(int entryCount, int from, int count)
    {
        int? nextToken;
        
        if (entryCount <= count + from) nextToken = null;
        else nextToken = count + from;

        int? previousToken;
        if (from > 0) previousToken = from - 1;
        else previousToken = null;
        
        return (previousToken, nextToken);
    }

    public static GameLevel[] PaginateLevels(IQueryable<GameLevel> levels, int from, int count)
    {
        return levels.AsEnumerable().Skip(from).Take(count).ToArray();
    }
    
    public static GameUser[] PaginateUsers(IQueryable<GameUser> users, int from, int count)
    {
        return users.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static GameAlbum[] PaginateAlbums(IQueryable<GameAlbum> albums, int from, int count)
    {
        return albums.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static GameIp[] PaginateIpAddresses(IQueryable<GameIp> addresses, int from, int count)
    {
        return addresses.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static LeaderboardEntry[] PaginateLeaderboardEntries(IQueryable<LeaderboardEntry> entries, int from,
        int count)
    {
        return entries.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static Report[] PaginateReports(IQueryable<Report> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static Punishment[] PaginatePunishments(IQueryable<Punishment> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static DailyLevel[] PaginateDailyLevels(IQueryable<DailyLevel> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static NewsEntry[] PaginateNews(IQueryable<NewsEntry> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(count).ToArray();
    }

    public static GameEvent[] PaginateEvents(IQueryable<GameEvent> events, int from, int count)
    {
        return events.AsEnumerable().Skip(from).Take(count).ToArray();
    }
}
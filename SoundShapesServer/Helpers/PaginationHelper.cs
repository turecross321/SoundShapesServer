using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class PaginationHelper
{
    private const int MaxEntryCount = 100;
    
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
        return levels.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }
    
    public static GameUser[] PaginateUsers(IQueryable<GameUser> users, int from, int count)
    {
        return users.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static GameAlbum[] PaginateAlbums(IQueryable<GameAlbum> albums, int from, int count)
    {
        return albums.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static IpAuthorization[] PaginateIpAddresses(IQueryable<IpAuthorization> addresses, int from, int count)
    {
        return addresses.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static LeaderboardEntry[] PaginateLeaderboardEntries(IQueryable<LeaderboardEntry> entries, int from,
        int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static Report[] PaginateReports(IQueryable<Report> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static Punishment[] PaginatePunishments(IQueryable<Punishment> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static DailyLevel[] PaginateDailyLevels(IQueryable<DailyLevel> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static NewsEntry[] PaginateNews(IQueryable<NewsEntry> entries, int from, int count)
    {
        return entries.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }

    public static GameEvent[] PaginateEvents(IQueryable<GameEvent> events, int from, int count)
    {
        return events.AsEnumerable().Skip(from).Take(Math.Min(count, MaxEntryCount)).ToArray();
    }
}
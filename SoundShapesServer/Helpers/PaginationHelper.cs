using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class PaginationHelper
{
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
}
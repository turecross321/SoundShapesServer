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
}
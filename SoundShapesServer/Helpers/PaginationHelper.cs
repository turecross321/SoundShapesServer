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
using SoundShapesServer.Database;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions;

public static class StringExtensions
{
    public static bool? ToBool(this string? input)
    {
        if (input == null)
            return null;

        if (bool.TryParse(input, out bool result))
        {
            return result;
        }

        return null;
    }
    
    public static int? ToInt(this string? input)
    {
        if (input == null)
            return null;

        if (int.TryParse(input, out int result))
        {
            return result;
        }

        return null;
    }
}
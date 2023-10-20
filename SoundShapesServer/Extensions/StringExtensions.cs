using SoundShapesServer.Database;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions;

public static class StringExtensions
{
    public static DateTimeOffset? ToDateFromUnix(this string? input)
    {
        if (input == null)
            return null;
    
        if (long.TryParse(input, out long unix))
        {
            return DateTimeOffset.FromUnixTimeSeconds(unix);
        }

        return null;
    }

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


    public static T? ToEnum<T>(this string? input) where T : struct, Enum
    {
        if (input == null)
            return null;

        if (!Enum.TryParse(input, out T result)) 
            return null;
        return result;
    }
    public static List<T>? ToEnumList<T>(this string? input) where T : struct, Enum
    {
        if (input == null)
            return null;

        List<T> list = new ();
        list.AddRange(input.Split(",").Select(value => Enum.TryParse(value, out T result) ? result : default));
        return list;
    }

    public static GameUser? ToUser(this string? input, GameDatabaseContext database)
    {
        return input == null ? null : database.GetUserWithId(input);
    }
    
    public static GameLevel? ToLevel(this string? input, GameDatabaseContext database)
    {
        return input == null ? null : database.GetLevelWithId(input);
    }
    
    public static LeaderboardEntry? ToLeaderboardEntry(this string? input, GameDatabaseContext database)
    {
        return input == null ? null : database.GetLeaderboardEntry(input);
    }
    
    public static GameAlbum? ToAlbum(this string? input, GameDatabaseContext database)
    {
        return input == null ? null : database.GetAlbumWithId(input);
    }

    public static List<GameUser>? ToUsers(this string? input, GameDatabaseContext database)
    {
        if (input == null)
            return null;
        
        List<GameUser> list = new ();
        foreach (string id in input.Split(","))
        {
           GameUser? user = database.GetUserWithId(id);
           if (user != null)
            list.Add(user);
        }

        return list;
    }
}
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

    public static DateTimeOffset? ToDate(this string? input)
    {
        if (input == null)
            return null;
        
        if (DateTimeOffset.TryParse(input, out DateTimeOffset result))
        {
            return result;
        }

        return null;
    }
}
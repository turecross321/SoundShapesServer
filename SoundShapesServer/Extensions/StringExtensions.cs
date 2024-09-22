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
    
    public static uint? ToUInt(this string? input)
    {
        if (input == null)
            return null;

        if (uint.TryParse(input, out uint result))
        {
            return result;
        }

        return null;
    }

    public static DateTime? ToDate(this string? input)
    {
        if (input == null)
            return null;
        
        if (DateTime.TryParse(input, out DateTime result))
        {
            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }

        return null;
    }
}
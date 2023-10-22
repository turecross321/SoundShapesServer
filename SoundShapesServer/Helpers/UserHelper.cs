namespace SoundShapesServer.Helpers;

public static partial class UserHelper
{
    [System.Text.RegularExpressions.GeneratedRegex("^[A-Za-z][A-Za-z0-9-_]{2,15}$")]
    private static partial System.Text.RegularExpressions.Regex UsernameRegex();
    public static bool IsUsernameLegal(string username)
    {
        return UsernameRegex().IsMatch(username);
    }
}
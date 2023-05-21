using static System.Text.RegularExpressions.Regex;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    private const string UsernameRegex = "^[A-Za-z][A-Za-z0-9-_]{2,15}$";
    public static bool IsUsernameLegal(string username)
    {
        return IsMatch(username, UsernameRegex);
    }
}
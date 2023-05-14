using System.Security.Cryptography;
using System.Text;
using static System.Text.RegularExpressions.Regex;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    private const string UsernameRegex = "^[A-Za-z][A-Za-z0-9-_]{2,15}$";
    public static bool IsUsernameLegal(string username)
    {
        return IsMatch(username, UsernameRegex);
    }
    
    public static string HashString(string input)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        using SHA512 hash = System.Security.Cryptography.SHA512.Create();
        byte[] hashedInputBytes = hash.ComputeHash(bytes);

        // Convert to text
        // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
        StringBuilder hashedInputStringBuilder = new System.Text.StringBuilder(128);
        foreach (byte b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));
        return hashedInputStringBuilder.ToString();
    }
}
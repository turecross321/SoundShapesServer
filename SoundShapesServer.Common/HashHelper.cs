using System.Security.Cryptography;
using System.Text;

namespace SoundShapesServer.Common;

public static class HashHelper
{
    public static string ComputeSha512Hash(string input)
    {
        using SHA512 sha512 = SHA512.Create();
        
        // Convert the input string to a byte array
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            
        // Compute the hash
        byte[] hashBytes = sha512.ComputeHash(inputBytes);
            
        // Convert the byte array to a hexadecimal string
        StringBuilder sb = new StringBuilder();
        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }
            
        return sb.ToString();
    }
}
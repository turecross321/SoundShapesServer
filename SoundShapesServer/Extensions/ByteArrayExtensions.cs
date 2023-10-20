using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json.Linq;

namespace SoundShapesServer.Extensions;

public static class ByteArrayExtensions
{
    public static bool IsPng(this byte[] bytes)
    {
        if (bytes.Length > 7 &&
            bytes[0] == 137 &&
            bytes[1] == 80 &&
            bytes[2] == 78 &&
            bytes[3] == 71 &&
            bytes[4] == 13 &&
            bytes[5] == 10 &&
            bytes[6] == 26 &&
            bytes[7] == 10)
            return true;

        return false;
    }
    
    public static JObject? LevelFileToJObject(this byte[] level)
    {
        using MemoryStream stream = new(level);
        string? json = DecompressZlib(stream);
        if (json == null) return null;
        
        return JObject.Parse(json);
    }
    private static string? DecompressZlib(Stream compressedStream)
    {
        using InflaterInputStream inflater = new(compressedStream);
        using MemoryStream outputMemoryStream = new();
        byte[] buffer = new byte[4096];
        try
        {
            int bytesRead;
            while ((bytesRead = inflater.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputMemoryStream.Write(buffer, 0, bytesRead);
            }
        }
        catch (Exception)
        {
            return null;
        }

        outputMemoryStream.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(outputMemoryStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }
}
using System.Text;
using HttpMultipartParser;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Helpers;

public static class ResourceHelper
{
    public static bool IsByteArrayPng(byte[] bytes)
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
    public static byte[] FilePartToBytes(FilePart filePart)
    {
        MemoryStream memoryStream = new();
        filePart.Data.CopyTo(memoryStream);
        byte[] bytes = memoryStream.ToArray();

        return bytes;
    }
    public static FileType GetFileTypeFromFilePart(FilePart file)
    {
        if (file.ContentType == "image/png") return FileType.Image;
        if (file.ContentType == "application/vnd.soundshapes.level") return FileType.Level;
        if (file.ContentType == "application/vnd.soundshapes.sound") return FileType.Sound;
            
        return FileType.Unknown;
    }

    public static JObject LevelFileToJObject(byte[] level)
    {
        using MemoryStream stream = new(level);
        return JObject.Parse(DecompressZlib(stream));
    }
    private static string DecompressZlib(MemoryStream compressedStream)
    {
        using InflaterInputStream inflater = new(compressedStream);
        using MemoryStream outputMemoryStream = new();
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = inflater.Read(buffer, 0, buffer.Length)) > 0)
        {
            outputMemoryStream.Write(buffer, 0, bytesRead);
        }

        outputMemoryStream.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(outputMemoryStream, Encoding.UTF8);
        return reader.ReadToEnd();
    }

    public static FileType GetFileTypeFromName(string name)
    {
        if (name is "image" or "thumbnail") return FileType.Image;
        if (name == "level") return FileType.Level;
        if (name == "sound") return FileType.Sound;

        return FileType.Unknown;
    }

    private const string LevelsPath = "levels";
    public static string GetLevelResourceKey(string levelId, FileType fileType)
    {
        return fileType switch
        {
            FileType.Image => $"{LevelsPath}/{levelId}-thumbnail",
            FileType.Level => $"{LevelsPath}/{levelId}-level",
            FileType.Sound => $"{LevelsPath}/{levelId}-sound",
            FileType.Unknown => throw new Exception(),
            _ => throw new Exception()
        };
    }

    private const string AlbumsPath = "albums";
    public static string GetAlbumResourceKey(string albumId, AlbumResourceType resourceType)
    {
        return resourceType switch
        {
            AlbumResourceType.Thumbnail => $"{AlbumsPath}/{albumId}-thumbnail",
            AlbumResourceType.SidePanel => $"{AlbumsPath}/{albumId}-sidePanel",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static string GenerateAlbumResourceUrl(string albumId, AlbumResourceType type)
    {
        return $"otg/~album:{albumId}/~content:{type.ToString()}/data.get";
    }
    
    private const string SavesPath = "saves";

    public static string GetSaveResourceKey(string userId)
    {
        return $"{SavesPath}/{userId}-save";
    }

    private const string NewsPath = "news";

    public static string GetNewsResourceKey(string id)
    {
        return $"{NewsPath}/{id}-thumbnail";
    }
}
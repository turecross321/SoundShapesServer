using HttpMultipartParser;
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

    public static FileType GetFileTypeFromName(string name)
    {
        if (name == "image") return FileType.Image;
        if (name == "level") return FileType.Level;
        if (name == "sound") return FileType.Sound;

        return FileType.Unknown;
    }

    private const string LevelsPath = "levels";
    public static string GetLevelResourceKey(string levelId, FileType fileType)
    {
        if (fileType == FileType.Image) return $"{LevelsPath}/{levelId}.png";
        if (fileType == FileType.Level) return $"{LevelsPath}/{levelId}.level";
        if (fileType == FileType.Sound) return $"{LevelsPath}/{levelId}.sound";

        return "";
    }
    
    private const string AlbumsPath = "albums";
    public static string GetAlbumResourceKey(string albumId, AlbumResourceType resourceType)
    {
        return resourceType switch
        {
            AlbumResourceType.Thumbnail => $"{AlbumsPath}/{albumId}_thumbnail.png",
            AlbumResourceType.SidePanel => $"{AlbumsPath}/{albumId}_sidePanel.png",
            _ => ""
        };
    }
    
    public static string GenerateAlbumResourceUrl(string albumId, AlbumResourceType type, string? sessionId)
    {
        return $"otg/~album:{albumId}/~content:{type.ToString()}/data.get/{sessionId}";
    }
    
    private const string SavesPath = "saves";

    public static string GetSaveResourceKey(string userId)
    {
        return $"{SavesPath}/{userId}.json";
    }

    private const string NewsPath = "news";

    public static string GetNewsResourceKey(string id)
    {
        return $"{NewsPath}/{id}.png";
    }
}
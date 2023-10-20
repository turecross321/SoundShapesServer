using System.Security.Cryptography;
using System.Text;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class ResourceHelper
{
    public static string HashString(string input)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return HashFile(bytes);
    }

    private static string HashFile(byte[] file)
    {
        using SHA512 hash = SHA512.Create();
        byte[] hashedInputBytes = hash.ComputeHash(file);

        // Convert to text
        // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
        StringBuilder hashedInputStringBuilder = new (128);
        foreach (byte b in hashedInputBytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));
        return hashedInputStringBuilder.ToString();
    }

    public static FileType GetFileTypeFromName(string name)
    {
        if (name is "image" or "thumbnail") return FileType.Image;
        if (name == "level") return FileType.Level;
        if (name == "sound") return FileType.Sound;

        return FileType.Unknown;
    }

    private const string LevelsPath = "levels";
    public static string GetLevelResourceKey(GameLevel level, FileType fileType)
    {
        string path = $"{LevelsPath}/{level.Id[0]}/{level.Id}";

        return fileType switch
        {
            FileType.Level => path + "-level",
            FileType.Image => path + "-thumbnail",
            FileType.Sound => path + "-sound",
            FileType.Unknown => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null),
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }

    private const string AlbumsPath = "albums";
    public static string GetAlbumResourceKey(string albumId, AlbumResourceType resourceType)
    {
        return resourceType switch
        {
            AlbumResourceType.Thumbnail => $"{AlbumsPath}/{albumId}-thumbnail",
            AlbumResourceType.SidePanel => $"{AlbumsPath}/{albumId}-sidePanel",
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
        };
    }

    public static string GetAlbumResourceUrl(string albumId, AlbumResourceType resourceType)
    {
        return $"~album:{albumId}/~content:{AlbumHelper.GetStringFromAlbumResourceType(resourceType)}/data";
    }

    private const string CommunityTabsPath = "communityTabs";

    public static string GetCommunityTabResourceKey(string id)
    {
        return $"{CommunityTabsPath}/{id}/-thumbnail";
    }
    public static string GetCommunityTabThumbnailUrl(string id)
    {
        return $"~communityTab:{id}/~content:thumbnail/data";
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
    
    public static string GetNewsThumbnailUrl(string id)
    {
        return $"~news:{id}/~content:thumbnail/data";
    }
}
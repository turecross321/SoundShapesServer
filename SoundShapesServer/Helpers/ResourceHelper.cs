using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ResourceHelper
{
    private const string LevelsPath = "levels";
    public static string GetLevelResourceKey(string levelId, string fileType)
    {
        if (fileType.Contains("image")) return $"{LevelsPath}/{levelId}.png";
        if (fileType.Contains("level")) return $"{LevelsPath}/{levelId}.level";
        if (fileType.Contains("sound")) return $"{LevelsPath}/{levelId}.sound";

        return "";
    }
    
    private const string AlbumsPath = "albums";
    public static string GetAlbumResourceKey(string albumId, string file)
    {
        if (Enum.TryParse(file, out AlbumResourceType type) == false) return "";

        switch (type)
        {
            case AlbumResourceType.thumbnail:
                return $"{AlbumsPath}/{albumId}_thumbnail.png";
            case AlbumResourceType.sidePanel:
                return $"{AlbumsPath}/{albumId}_sidePanel.png";
        }

        return "";
    }
    
    public static string GenerateAlbumResourceUrl(string albumId, AlbumResourceType type, string sessionId)
    {
        return $"otg/~album:{albumId}/~content:{type.ToString()}/data.get/{sessionId}";
    }
    
    private const string SavesPath = "saves";

    public static string GetSaveResourceKey(string userId)
    {
        return $"{SavesPath}/{userId}";
    }
}
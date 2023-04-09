using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ResourceHelper
{
    private const string levelsPath = "levels";
    public static string GetLevelResourceKey(string levelId, string fileType)
    {
        if (fileType.Contains("image")) return $"{levelsPath}/{levelId}.png";
        if (fileType.Contains("level")) return $"{levelsPath}/{levelId}.level";
        if (fileType.Contains("sound")) return $"{levelsPath}/{levelId}.sound";

        return "";
    }
    
    private const string albumsPath = "albums";
    public static string GetAlbumResourceKey(string albumId, string file)
    {
        if (Enum.TryParse(file, out AlbumResourceType type) == false) return "";

        switch (type)
        {
            case AlbumResourceType.thumbnail:
                return $"{albumsPath}/{albumId}_thumbnail.png";
            case AlbumResourceType.sidePanel:
                return $"{albumsPath}/{albumId}_sidePanel.png";
        }

        return "";
    }
    
    public static string GenerateAlbumResourceUrl(string albumId, AlbumResourceType type, string sessionId)
    {
        return $"otg/~album:{albumId}/~content:{type.ToString()}/data.get/{sessionId}";
    }
    
    private const string savesPath = "saves";

    public static string GetSaveResourceKey(string userId)
    {
        return $"{savesPath}/{userId}";
    }
}
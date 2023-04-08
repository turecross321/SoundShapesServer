namespace SoundShapesServer.Helpers;

public class IdFormatter
{
    public static string FormatLevelId(string id)
    {
        return $"/~level:{id}";
    }
    public static string FormatLevelIdAndVersion(string id, long version)
    {
        return $"/~level:{id}/~version:{version}";
    }
    public static string FormatLevelPublishId(string id, long creationTime)
    {
        return $"/~level:{id}/~upload:{creationTime}";
    }
    
    public static string FormatUserId(string id)
    {
        return $"/~identity:{id}";
    }

    public static string FormatFollowId(string followerId, string followedId)
    {
        return $"/~identity:{followerId}/~follow:/~identity:{followedId}";
    }

    public static string FormatAlbumId(string albumId)
    {
        return $"/~album:{albumId}";
    }
    public static string FormatAlbumLinkId(string albumId, string levelId)
    {
        // $"/~album:{albumId}/~link:/~level:{levelId}";
        return $"/~album:{albumId}/~link:%2F%7Elevel%3A{levelId}";
    }

    public static string FormatVersionId(string id)
    {
        return $"~version:{id}";
    }
}
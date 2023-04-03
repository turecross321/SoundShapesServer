using SoundShapesServer.Enums;

namespace SoundShapesServer.Helpers;

public class IdFormatter
{
    public static string FormatLevelId(string id)
    {
        return $"/~level:{id}";
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
}
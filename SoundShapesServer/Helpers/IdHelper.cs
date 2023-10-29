using MongoDB.Bson;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class IdHelper
{
    public static string FormatLevelId(string id)
    {
        return $"/~level:{id}";
    }
    public static string FormatLevelIdAndVersion(GameLevel level)
    {
        string id = level.Id;
        long version = level.ModificationDate.ToUnixTimeMilliseconds();
        return $"/~level:{id}/~version:{version}";
    }

    public static string FormatRelationLevelId(string userId, string levelId)
    {
        // yes, you're reading this correctly. both liked and queued levels should have queued in the id
        return "/~identity:" + userId + "/~queued:/~level:" + levelId;
    }

    public static string DeFormatLevelIdAndVersion(string formattedId)
    {
        return formattedId.Split(":")[1].Split("/")[0];
    }

    public static string DeFormatIdentityId(string formattedId)
    {
        // /~identity:42e88fdd-17e0-4a85-96a5-896c71584b8a
        return formattedId.Remove(0, 11);
    }
    public static string FormatLevelPublishId(string id, long creationTime)
    {
        return $"/~level:{id}/~upload:{creationTime}";
    }
    
    public static string FormatUserId(string id)
    {
        return $"/~identity:{id}";
    }

    public static string FormatFollowId(string followerId, string recipientId)
    {
        return $"/~identity:{followerId}/~follow:/~identity:{recipientId}";
    }

    public static string FormatAlbumId(string albumId)
    {
        return $"/~album:{albumId}";
    }
    public static string FormatAlbumLinkId(string albumId, string levelId)
    {
        return $"/~album:{albumId}/~link:/~level:{levelId}";
    }

    public static string FormatVersionId(DateTimeOffset date)
    {
        return $"~version:{date.ToUnixTimeMilliseconds().ToString()}";
    }

    /// <summary>
    /// Used in migrations to convert old string guid IDs to object IDs 
    /// </summary>
    public static ObjectId TrimToObjectId(string id)
    {
        id = new string(id.Where(c => c != '-').Take(24).ToArray());
        return ObjectId.Parse(id);
    }
}
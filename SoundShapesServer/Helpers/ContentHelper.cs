using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ContentHelper
{
    public static string GetContentTypeString(GameContentType contentType)
    {
        return contentType switch
        {
            GameContentType.Identity => "identity",
            GameContentType.Level => "level",
            GameContentType.Album => "album",
            GameContentType.Upload => "upload",
            GameContentType.Alias => "alias",
            GameContentType.Like => "like",
            GameContentType.Queued => "queued",
            GameContentType.Follow => "follow",
            GameContentType.Link => "link",
            GameContentType.Version => "version",
            GameContentType.Activity => "activity",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
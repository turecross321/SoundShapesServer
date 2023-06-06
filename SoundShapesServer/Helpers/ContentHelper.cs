using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class ContentHelper
{
    public static string GetContentTypeString(GameContentType contentType)
    {
        return contentType switch
        {
            GameContentType.User => "identity",
            GameContentType.Level => "level",
            GameContentType.Album => "album",
            GameContentType.PublishedLevel => "upload",
            GameContentType.RemovedLevelAuthor => "alias",
            GameContentType.Like => "like",
            GameContentType.Queued => "queued",
            GameContentType.Follow => "follow",
            GameContentType.Link => "link",
            GameContentType.Version => "version",
            GameContentType.Event => "activity",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
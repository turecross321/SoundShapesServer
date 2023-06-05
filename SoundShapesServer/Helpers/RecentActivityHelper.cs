using SoundShapesServer.Types.PlayerActivity;

namespace SoundShapesServer.Helpers;

public static class RecentActivityHelper
{
    public static string EventEnumToGameString(EventType type)
    {
        return type switch
        {
            EventType.Publish => "publish",
            EventType.Follow => "follow",
            EventType.Like => "like",
            _ => EventEnumToGameString(EventType.Publish)
        };
    }
}
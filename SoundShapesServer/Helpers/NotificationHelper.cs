using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Notifications;

namespace SoundShapesServer.Helpers;

public static class NotificationHelper
{
    public static List<NotificationType> GetNotificationTypesFromEvent(GameDatabaseContext database, GameEvent gameEvent)
    {
        List<NotificationType> types = new List<NotificationType>();
        
        switch (gameEvent.EventType)
        {
            case EventType.LevelLike:
                types.Add(NotificationType.LevelLiked);
                break;
            case EventType.ScoreSubmission:
                if (playMilestones.Contains(gameEvent.ContentLevel!.UniquePlaysCount))
                    types.Add(NotificationType.LevelPlayMilestone);
                if (gameEvent.ContentLeaderboardEntry!.Position() == 0)
                    types.Add(NotificationType.TopScoreBeaten);
                break;
            case EventType.AlbumCreation:
                types.Add(NotificationType.AddedToAlbum);
                break;
            case EventType.DailyCreation:
                // check if level was added to daily
                types.Add(NotificationType.AddedToDailyLevels);
                break;
            case EventType.LevelPublish:
                types.Add(NotificationType.FollowingPublishedLevel);
                break;
            case EventType.NewsCreation:
                // check if there are any mentions
                types.Add(NotificationType.MentionedInNews);
                break;
            case EventType.UserFollow:
                types.Add(NotificationType.StartedFollowing);
                break;
        }

        return types;
    }

    private static readonly int[] playMilestones =
    {
        50, 100, 200, 500, 1000
    };
}

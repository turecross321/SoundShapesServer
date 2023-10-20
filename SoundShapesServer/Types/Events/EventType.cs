namespace SoundShapesServer.Types.Events;

// ReSharper disable InconsistentNaming
public enum EventType
{
    LevelPublish = 0,
    LevelLike = 1,
    LevelQueue = 2,
    UserFollow = 3,
    ScoreSubmission = 4,
    AccountRegistration = 5,
    AlbumCreation = 6,
    DailyCreation = 7,
    NewsCreation = 8
}
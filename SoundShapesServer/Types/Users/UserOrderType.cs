using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Users;

public enum UserOrderType
{
    [OrderType("followers", "Total amount of followers")]
    Followers,
    [OrderType("following", "Total amount of users that user is following")]
    Following,
    [OrderType("publishedLevels", "Total amount of published levels")]
    PublishedLevels,
    [OrderType("likedLevels", "Total amount of liked levels")]
    LikedLevels,
    [OrderType("creationDate", "Creation date")]
    CreationDate,
    [OrderType("playedLevels", "Total amount of played levels")]
    PlayedLevels,
    [OrderType("completedLevels", "Total amount of completed levels")]
    CompletedLevels,
    [OrderType("deaths", "Total amount of deaths")]
    Deaths,
    [OrderType("playTime", "Total amount of play time")]
    PlayTime,
    [OrderType("lastGameLogin", "Date of last game login")]
    LastGameLogin,
    [OrderType("events", "Total amount of events")]
    Events,
    DoNotOrder
}
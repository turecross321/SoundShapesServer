using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Leaderboard;

public enum LeaderboardOrderType
{
    [OrderType("score", "First by notes, and then by play time.")]
    Score,
    [OrderType("playTime", "Play time.")]
    PlayTime,
    [OrderType("notes", "Amount of collected notes.")]
    Notes,
    [OrderType("creationDate", "Creation date.")]
    CreationDate
}
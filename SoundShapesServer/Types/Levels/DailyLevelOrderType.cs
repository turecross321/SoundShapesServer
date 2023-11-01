using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Levels;

public enum DailyLevelOrderType
{
    [OrderType("date", "Date that level was picked for")]
    Date
}
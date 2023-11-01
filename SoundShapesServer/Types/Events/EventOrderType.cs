using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Events;

public enum EventOrderType
{
    [OrderType("creationDate", "Creation date")]
    CreationDate
}
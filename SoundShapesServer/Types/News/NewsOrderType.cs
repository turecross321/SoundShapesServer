using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.News;

public enum NewsOrderType
{
    [OrderType("creationDate", "Creation date")]
    CreationDate,
    [OrderType("modificationDate", "Modification date")]
    ModificationDate,
    [OrderType("characters", "Total amount of characters")]
    Characters
}
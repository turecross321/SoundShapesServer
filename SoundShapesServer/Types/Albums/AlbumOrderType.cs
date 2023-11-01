using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Albums;

public enum AlbumOrderType
{
    [OrderType("creationDate", "Creation date.")]
    CreationDate,
    [OrderType("modificationDate", "Modification date.")]
    ModificationDate,
    [OrderType("plays", "Total amount of plays throughout all the levels.")]
    Plays,
    [OrderType("uniquePlays", "Total amount of unique plays throughout all the levels.")]
    UniquePlays,
    [OrderType("levels", "Total amount of levels.")]
    Levels,
    [OrderType("fileSize", "Combined file size of all the levels.")]
    FileSize,
    [OrderType("difficulty", "Total difficulty throughout all the levels.")]
    Difficulty
}
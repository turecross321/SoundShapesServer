using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Levels;

public enum LevelOrderType
{
    [OrderType("creationDate", "Creation date")]
    CreationDate,
    [OrderType("modificationDate", "Modification date")]
    ModificationDate,
    [OrderType("plays", "Total amount of plays")]
    Plays,
    [OrderType("uniquePlays", "Total amount of unique plays")]
    UniquePlays,
    [OrderType("uniqueCompletions", "Total amount of unique completions")]
    UniqueCompletions,
    [OrderType("completions", "Total amount of completions")]
    Completions,
    [OrderType("likes", "Total amount of likes")]
    Likes,
    [OrderType("queues", "Total amount of queues")]
    Queues,
    [OrderType("fileSize", "File size of level file")]
    FileSize,
    [OrderType("difficulty", "Difficulty")]
    Difficulty,
    [OrderType("random", "Random - uses current date as seed")]
    Random,
    [OrderType("deaths", "Total amount of deaths")]
    Deaths,
    [OrderType("playTime", "Total play time")]
    PlayTime,
    [OrderType("averagePlayTime", "Total play time divided by total plays")]
    AveragePlayTime,
    [OrderType("screens", "Total amount of screens in level")]
    Screens,
    [OrderType("entities", "Total amount of entities in level")]
    Entities,
    [OrderType("bpm", "BPM")]
    Bpm,
    [OrderType("transposeValue", "Transpose value")]
    TransposeValue,
    DoNotOrder
}
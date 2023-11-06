namespace SoundShapesServer.Types.Levels.Importing;

public class ImportLevelData
{
    public DateTimeOffset LevelWriteDate { get; init; }
    public byte[]? Thumbnail { get; init; }
    public byte[]? LevelFile { get; init; }
    public byte[]? SoundFile { get; init; }
}
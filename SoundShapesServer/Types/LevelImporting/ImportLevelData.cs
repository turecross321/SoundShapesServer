namespace SoundShapesServer.Types.LevelImporting;

public class ImportLevelData
{
    public DateTimeOffset CreationDate { get; set; }
    public byte[] Thumbnail { get; set; }
    public byte[] LevelFile { get; set; }
    public byte[] SoundFile { get; set; }
}
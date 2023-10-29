namespace SoundShapesServer.Types.Levels;

internal class ImportedLevelDependency
{
    public string LevelIdentifier { get; set; }
    public FileType FileType { get; set; }
    public string FilePath { get; set; }
}
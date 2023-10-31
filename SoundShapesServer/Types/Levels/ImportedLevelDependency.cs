namespace SoundShapesServer.Types.Levels;

internal class ImportedLevelDependency
{
    public string LevelIdentifier { get; init; }
    public FileType FileType { get; init; }
    public string FilePath { get; init; }
}
namespace SoundShapesServer.Types.Levels;

internal class ImportedLevelDependency
{
    public ImportedLevelDependency(string levelIdentifier, FileType fileType, string filePath)
    {
        LevelIdentifier = levelIdentifier;
        FileType = fileType;
        FilePath = filePath;
    }

    public string LevelIdentifier { get; set; }
    public FileType FileType { get; set; }
    public string FilePath { get; set; }
}
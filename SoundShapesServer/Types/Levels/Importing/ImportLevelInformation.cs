namespace SoundShapesServer.Types.Levels.Importing;

public class ImportLevelInformation
{
    public required string Id { get; init; }
    public string? Name { get; init; }
    public required bool CampaignLevel { get; init; }
    public required bool UploadIfMissingFiles { get; init; }
    public string LevelFilePath { get; set; } = null!;
    public string ThumbnailFilePath { get; set; } = null!;
    public string? SoundFilePath { get; set; }
}
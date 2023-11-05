namespace SoundShapesServer.Types.LevelImporting;

public class ImportLevelInformation
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public bool CampaignLevel { get; set; }
    public string LevelFilePath { get; set; }
    public string ThumbnailFilePath { get; set; }
    public string? SoundFilePath { get; set; }
}
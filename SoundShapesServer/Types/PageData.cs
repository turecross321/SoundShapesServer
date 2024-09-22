namespace SoundShapesServer.Types;

public class PageData
{
    public required uint Skip { get; set; }
    public required uint Take { get; set; }
    public required DateTime? MinimumCreationDate { get; set; }
    public required DateTime? MaximumCreationDate { get; set; }
    public required string[] ExcludeIds { get; set; }
}
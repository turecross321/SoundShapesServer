namespace SoundShapesServer.Types;

public class PageData
{
    public required int Skip { get; set; }
    public required int Take { get; set; }
    public required string? FromId { get; set; }
    public required DateTime? MinimumCreationDate { get; set; }
    public required DateTime? MaximumCreationDate { get; set; }
    public required string[] ExcludeIds { get; set; }
}
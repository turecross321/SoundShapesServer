namespace SoundShapesServer.Types;

public class PageData
{
    public required int Skip { get; set; }
    public required int Take { get; set; }
    public required string? FromId { get; set; }
    public required DateTimeOffset? MinimumCreationDate { get; set; }
    public required DateTimeOffset? MaximumCreationDate { get; set; }
    public required string[] ExcludeIds { get; set; }
}
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.News;

public class NewsFilters
{
    public string? Language { get; set; }
    public GameUser[]? Authors { get; set; }
}
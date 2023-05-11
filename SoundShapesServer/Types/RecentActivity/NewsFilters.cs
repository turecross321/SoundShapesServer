using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.RecentActivity;

public class NewsFilters
{
    public NewsFilters(string? language = null, GameUser[]? authors = null)
    {
        Language = language;
        Authors = authors;
    }
    public string? Language { get; set; }
    public GameUser[]? Authors { get; set; }
}
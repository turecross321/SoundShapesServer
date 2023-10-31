using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.News;

public class NewsFilters
{
    [DocPropertyQuery("language", "Filters out entries that are not in specified language.")] 
    public string? Language { get; init; }
    [DocPropertyQuery("authors", "Filters out entries that were not written any of the specified users.")]
    public GameUser[]? Authors { get; set; }
}
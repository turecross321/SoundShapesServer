using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.News;

public class NewsFilters : IFilters
{
    [FilterProperty("language", "Filters out entries that are not in specified language.")]
    public string? Language { get; init; }

    [FilterProperty("authors", "Filters out entries that were not written any of the specified users.")]
    public GameUser[]? Authors { get; set; }
}
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Responses.Api.News;

public class ApiNewsWrapper
{
    public ApiNewsWrapper(IEnumerable<NewsEntry> entries, int totalEntries)
    { 
        Entries = entries.Select(l=> new ApiNewsResponse(l)).ToArray();
        Count = totalEntries;
    }

    public ApiNewsResponse[] Entries { get; set; }
    public int Count { get; set; }
}
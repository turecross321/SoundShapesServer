using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiNewsWrapper
{
    public ApiNewsWrapper(NewsEntry[] entries, int totalEntries)
    { 
        Entries = entries.Select(l=> new ApiNewsResponse(l)).ToArray();
        Count = totalEntries;
    }

    public ApiNewsResponse[] Entries { get; set; }
    public int Count { get; set; }
}
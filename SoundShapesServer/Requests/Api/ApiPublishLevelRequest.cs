namespace SoundShapesServer.Requests.Api;

public class ApiPublishLevelRequest
{
    public ApiPublishLevelRequest(int language, string title, DateTimeOffset modified)
    {
        Language = language;
        Title = title;
        Modified = modified;
    }

    public string Title { get; }
    public int Language { get; }
    public DateTimeOffset? Modified { get; set; } 
}
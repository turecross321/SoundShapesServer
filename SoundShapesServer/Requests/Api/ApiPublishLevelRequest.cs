namespace SoundShapesServer.Requests.Api;

public class ApiPublishLevelRequest
{
    public ApiPublishLevelRequest(int language, string name, DateTimeOffset modified)
    {
        Language = language;
        Name = name;
        Modified = modified;
    }

    public string Name { get; }
    public int Language { get; }
    public DateTimeOffset? Modified { get; set; } 
}
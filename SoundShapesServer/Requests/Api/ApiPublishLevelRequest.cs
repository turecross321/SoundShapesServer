namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPublishLevelRequest
{
    public ApiPublishLevelRequest(int language, string name, DateTimeOffset modificationDate)
    {
        Language = language;
        Name = name;
        ModificationDate = modificationDate;
    }

    public string Name { get; set; }
    public int Language { get; }
    public DateTimeOffset? ModificationDate { get; set; } 
}
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiEditLevelRequest
{
    public ApiEditLevelRequest(int language, string name)
    {
        Language = language;
        Name = name;
    }

    public string Name { get; }
    public int Language { get; }
}
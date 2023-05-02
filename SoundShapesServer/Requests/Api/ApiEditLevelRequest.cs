namespace SoundShapesServer.Requests.Api;

// ReSharper disable once UnusedType.Global
public class ApiEditLevelRequest
{
    public ApiEditLevelRequest(int language, string name, DateTimeOffset modified)
    {
        Language = language;
        Name = name;
    }

    public string Name { get; }
    public int Language { get; }
}
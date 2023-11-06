using SoundShapesServer.Types.Levels;

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiEditLevelRequest
{
    public string Name { get; init; }
    public int Language { get; set; }
    public LevelVisibility Visibility { get; set; }
}
// ReSharper disable UnassignedGetOnlyAutoProperty

using SoundShapesServer.Types.Levels;

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateLevelRequest
{
    public string Name { get; init; }
    public int Language { get; }
    public DateTimeOffset? CreationDate { get; set; }
    public LevelVisibility Visibility { get; set; }
}
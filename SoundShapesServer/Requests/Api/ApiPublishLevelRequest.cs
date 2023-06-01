// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPublishLevelRequest
{
    public string Name { get; set; }
    public int Language { get; }
    public DateTimeOffset? ModificationDate { get; set; } 
}
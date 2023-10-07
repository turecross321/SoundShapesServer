// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateLevelRequest
{
    public string Name { get; set; }
    public int Language { get; }
    public long? CreationDate { get; set; } 
}
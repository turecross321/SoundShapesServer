// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

public class ApiCreateCommunityTabRequest
{
    public int ContentType { get; set; }
    public string ButtonLabel { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}
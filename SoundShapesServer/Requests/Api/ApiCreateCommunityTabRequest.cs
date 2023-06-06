// ReSharper disable ClassNeverInstantiated.Global

using SoundShapesServer.Types;
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Global

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

public class ApiCreateCommunityTabRequest
{
    public GameContentType ContentType { get; set; }
    public string ButtonLabel { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}
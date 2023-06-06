// ReSharper disable ClassNeverInstantiated.Global

using SoundShapesServer.Types;
// ReSharper disable UnassignedGetOnlyAutoProperty

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

public class ApiCreateCommunityTabRequest
{
    public GameContentType ContentType { get; }
    public string ButtonLabel { get; }
    public string Title { get; }
    public string Description { get; }
    public string Query { get; }
}
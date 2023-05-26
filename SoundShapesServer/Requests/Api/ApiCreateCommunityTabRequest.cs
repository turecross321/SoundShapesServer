// ReSharper disable ClassNeverInstantiated.Global
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

public class ApiCreateCommunityTabRequest
{
    public string ButtonLabel { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}
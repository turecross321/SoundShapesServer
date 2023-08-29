using SoundShapesServer.Types;

#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateCommunityTabRequest
{
    public GameContentType ContentType { get; set; }
    public string ButtonLabel { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Query { get; set; }
}
#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateAlbumRequest
{
    public string Name { get; set; }
    public string LinerNotes { get; set; }
    public string[] LevelIds { get; set; }
}
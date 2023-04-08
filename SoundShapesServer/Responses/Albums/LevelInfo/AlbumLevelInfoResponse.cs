using SoundShapesServer.Enums;

namespace SoundShapesServer.Responses.Albums;

public class AlbumLevelInfoResponse
{
    public string id { get; set; }
    public string type { get; } = ResponseType.link.ToString();
    public long timestamp { get; set; }
    public AlbumLevelInfoTarget target { get; set; }
}
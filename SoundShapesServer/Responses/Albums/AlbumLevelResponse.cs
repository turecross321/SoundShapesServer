using SoundShapesServer.Enums;

namespace SoundShapesServer.Responses.Albums;

public class AlbumLevelResponse
{
    public string id { get; set; }
    public string type = ResponseType.link.ToString();
    public long timestamp { get; set; }
    public AlbumLevelTarget target { get; set; }
}
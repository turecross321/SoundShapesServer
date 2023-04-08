using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Albums;

public class AlbumTarget
{
    public string id { get; set; }
    public string type = ResponseType.album.ToString();
    public AlbumMetadata metadata { get; set; }
}
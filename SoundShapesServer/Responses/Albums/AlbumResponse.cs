namespace SoundShapesServer.Responses.Albums;

public class AlbumResponse
{
    public string id { get; set; }
    public string type { get; set; }
    public string timestamp { get; set; }
    public AlbumTarget target { get; set; }
}
using HttpMultipartParser;

namespace SoundShapesServer.Requests.Game;

public class LevelPublishRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public byte[] Icon { get; set; }
    public byte[] Level { get; set; }
    public byte[] Song { get; set; }
    public int Language { get; set; }
    public string Id { get; set; }
    public long FileSize { get; set; }
}
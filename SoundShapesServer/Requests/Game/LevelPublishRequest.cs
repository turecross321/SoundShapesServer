using HttpMultipartParser;

namespace SoundShapesServer.Requests.Game;

public class LevelPublishRequest
{
    public string Title { get; set; }
    public int Language { get; set; }
    public string Id { get; set; }
    public long FileSize { get; set; }
}
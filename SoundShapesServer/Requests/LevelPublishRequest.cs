using HttpMultipartParser;

namespace SoundShapesServer.Requests;

public class LevelPublishRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public FilePart Icon { get; set; }
    public FilePart Level { get; set; }
    public FilePart Song { get; set; }
    public int Language { get; set; }
    public string Id { get; set; }
}
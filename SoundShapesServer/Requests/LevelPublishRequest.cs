using System.Net.Mime;
using HttpMultipartParser;

namespace SoundShapesServer.Requests;

public class LevelPublishRequest
{
    public string title { get; set; }
    public string description { get; set; }
    public FilePart image { get; set; }
    public FilePart level { get; set; }
    public FilePart sound { get; set; }
    public int sce_np_language { get; set; }
    public string levelId { get; set; }
}
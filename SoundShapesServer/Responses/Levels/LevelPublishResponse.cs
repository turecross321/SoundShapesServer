using SoundShapesServer.Enums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Levels;
public class LevelPublishResponse
{
    public string id { get; set; } //   format:  /~level:<actual-id>/upload:<creationTime>     WHAT THE FUCK
    public string type = ResponseType.upload.ToString();
    public LevelAuthor author { get; set; }
    public string title { get; set; }
    public IList<string> dependencies { get; set; }
    public string visibility { get; set; }
    public string description { get; set; }
    public ExtraData extraData { get; set; }
    public LevelParent parent { get; set; }
    public long creationTime { get; set; }
}
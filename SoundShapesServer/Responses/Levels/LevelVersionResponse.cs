using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Levels;

public class LevelVersionResponse
{
    public string id { get; set; }
    public string type = ResponseType.version.ToString();
}
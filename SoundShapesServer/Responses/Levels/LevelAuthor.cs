using Newtonsoft.Json;
using SoundShapesServer.Enums;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Levels;

public class LevelAuthor
{
    public string id { get; set; } //  format:   /~identity:<actual-id>
    public string type = ResponseType.identity.ToString();
    public string displayName { get; set; }
}
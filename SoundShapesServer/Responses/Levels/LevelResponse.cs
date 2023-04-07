using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Levels;

public class LevelResponse
{
    public string id { get; set; }
    public UserResponse author { get; set; }
    public string latestVersion { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string type { get; set; }
    public bool completed { get; set; }

    public LevelMetadataResponse metadata { get; set; }
}
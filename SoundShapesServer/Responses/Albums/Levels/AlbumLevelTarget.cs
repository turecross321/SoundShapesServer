using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Albums;

public class AlbumLevelTarget
{
    public string id { get; set; }
    public string type = ResponseType.level.ToString();
    public LevelVersionResponse latestVersion { get; set; }
    public UserResponse author { get; set; }
    public bool completed { get; set; }
    public LevelMetadataResponse metadata { get; set; }
}
using SoundShapesServer.Responses.Levels;

namespace SoundShapesServer.Types.Levels;

public class LevelPage
{
    public LevelResponse[] Items { get; set; }
    public int NextToken { get; set; }
    public int Count { get; set; }
}
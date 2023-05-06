using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelParentResponse
{
    public LevelParentResponse(GameLevel level)
    {
        Id = IdFormatter.FormatLevelId(level.Id);
    }

    public string Id { get; set; }
    public string Type = GameContentType.level.ToString();
}
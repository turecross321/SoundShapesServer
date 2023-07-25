using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelParentResponse : IResponse
{
    public LevelParentResponse(GameLevel level)
    {
        Id = IdHelper.FormatLevelId(level.Id);
    }

    public string Id { get; set; }
    public string Type = ContentHelper.GetContentTypeString(GameContentType.Level);
}
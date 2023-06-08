using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelVersionResponse
{
    public LevelVersionResponse(GameLevel level)
    {
        Id = IdHelper.FormatVersionId(level.ModificationDate.ToUnixTimeMilliseconds().ToString());
    }

    [JsonProperty("id")] public string Id { get; }
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Version);
}
using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelVersionResponse : IResponse
{
    public LevelVersionResponse(GameLevel level)
    {
        Id = IdHelper.FormatVersionId(level.ModificationDate);
    }

    public LevelVersionResponse() { }

    [JsonProperty("id")] public string Id { get; } = IdHelper.FormatVersionId(DateTimeOffset.UnixEpoch);
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Version);
}
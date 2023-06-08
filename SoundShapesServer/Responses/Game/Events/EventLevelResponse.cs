using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.IdHelper;

namespace SoundShapesServer.Responses.Game.Events;

public class EventLevelResponse
{
    public EventLevelResponse(GameLevel level)
    {
        Id = level.Id;
        LatestEventLevelVersionResponse = new EventLevelVersionResponse(FormatLevelIdAndVersion(level));
        Metadata = new LevelMetadataResponse(level);
        Author = new UserTargetResponse(level.Author);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("latestVersion")] public EventLevelVersionResponse LatestEventLevelVersionResponse { get; set; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; set; }
    [JsonProperty("author")] public UserTargetResponse Author { get; set; }
}
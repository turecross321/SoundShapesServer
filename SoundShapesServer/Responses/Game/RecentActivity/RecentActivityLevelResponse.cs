using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.IdFormatter;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class RecentActivityLevelResponse
{
    public RecentActivityLevelResponse(GameLevel level)
    {
        Id = level.Id;
        LatestVersion = new Version(FormatLevelIdAndVersion(level.Id, level.ModificationDate.ToUnixTimeSeconds()));
        Metadata = new LevelMetadataResponse(level);
        Author = new UserResponse(level.Author);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("latestVersion")] public Version LatestVersion { get; set; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; set; }
    [JsonProperty("author")] public UserResponse Author { get; set; }
}
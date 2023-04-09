using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("author")] public UserResponse Author { get; set; }
    [JsonProperty("latestVersion")] public string LatestVersion { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("completed")] public bool Completed { get; set; }

    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; set; }
}
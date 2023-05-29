using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.IdFormatter;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelTargetResponse
{
    public LevelTargetResponse(GameLevel level, GameUser user)
    {
        Id = FormatLevelId(level.Id);
        Author = new UserResponse(level.Author);
        LatestVersion = new LevelVersionResponse(level);
        Completed = level.UniqueCompletions.Contains(user);
        Liked = level.Likes.AsEnumerable().Select(r=>r.User).Contains(user);
        Metadata = new LevelMetadataResponse(level);
    }

    [JsonProperty("id")] public string Id { get; }
    [JsonProperty("author")] public UserResponse Author { get;  }
    [JsonProperty("latestVersion", NullValueHandling = NullValueHandling.Ignore)] public LevelVersionResponse LatestVersion { get; }
    [JsonProperty("type")] public string Type = GameContentType.level.ToString();
    [JsonProperty("completed")] public bool Completed { get; }
    [JsonProperty("liked")] public bool Liked { get; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; }
}
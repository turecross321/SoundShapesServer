using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.IdHelper;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelTargetResponse
{
    public LevelTargetResponse(GameLevel level, GameUser user)
    {
        Id = FormatLevelId(level.Id);
        Author = new UserTargetResponse(level.Author);
        LatestVersion = new LevelVersionResponse(level);
        Completed = level.UniqueCompletions.Contains(user);
        Liked = level.Likes.AsEnumerable().Select(r=>r.User).Contains(user);
        Metadata = new LevelMetadataResponse(level);
    }

    [JsonProperty("id")] public string Id { get; }
    [JsonProperty("author")] public UserTargetResponse Author { get;  }
    [JsonProperty("latestVersion", NullValueHandling = NullValueHandling.Ignore)] public LevelVersionResponse LatestVersion { get; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Level);
    [JsonProperty("completed")] public bool Completed { get; }
    [JsonProperty("liked")] public bool Liked { get; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; }
}
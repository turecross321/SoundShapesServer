using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.IdHelper;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelResponse : IResponse
{
    public LevelResponse(GameLevel level, GameUser? accessor)
    {
        Id = FormatLevelId(level.Id);
        Author = new UserTargetResponse(level.Author);
        LatestVersion = FormatLevelIdAndVersion(level);
        Title = level.Name;
        Metadata = new LevelMetadataResponse(level);
        Completed = accessor != null && level.UniqueCompletions.Contains(accessor);
    }

    [JsonProperty("id")] public string? Id { get; }
    [JsonProperty("author")] public UserTargetResponse Author { get;  }
    [JsonProperty("latestVersion", NullValueHandling = NullValueHandling.Ignore)] public string LatestVersion { get; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Level);
    [JsonProperty("completed")] public bool Completed { get; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; }
}
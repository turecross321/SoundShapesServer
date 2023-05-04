using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelResponse
{
    public LevelResponse(GameLevel level, GameUser user)
    {
        string formattedLevelId = IdFormatter.FormatLevelId(level.Id);

        Id = formattedLevelId;
        Author = new UserResponse(level.Author ?? new GameUser());
        LatestVersion = IdFormatter.FormatLevelIdAndVersion(level.Id, level.ModificationDate.ToUnixTimeMilliseconds());
        Title = level.Name;
        Type = GameContentType.level.ToString();
        Completed = level.UsersWhoHaveCompletedLevel.Contains(user);
        Metadata = new LevelMetadataResponse(level);
    }

    [JsonProperty("id")] public string Id { get; }
    [JsonProperty("author")] public UserResponse Author { get;  }
    [JsonProperty("latestVersion")] public string LatestVersion { get; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("type")] public string Type { get; }
    [JsonProperty("completed")] public bool Completed { get; }

    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; }
}
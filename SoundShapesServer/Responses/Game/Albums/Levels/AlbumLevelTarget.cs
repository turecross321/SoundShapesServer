using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelTarget
{
    public AlbumLevelTarget(GameLevel level, GameUser user)
    {
        Id = IdFormatter.FormatLevelId(level.Id);
        Completed = level.UsersWhoHaveCompletedLevel.Contains(user);
        LatestVersion = new LevelVersionResponse(level);
        Author = new UserResponse(level.Author);
        Metadata = new LevelMetadataResponse(level);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = GameContentType.level.ToString();
    [JsonProperty("latestVersion")] public LevelVersionResponse LatestVersion { get; set; }
    [JsonProperty("author")] public UserResponse Author { get; set; }
    [JsonProperty("completed")] public bool Completed { get; set; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; set; }
}
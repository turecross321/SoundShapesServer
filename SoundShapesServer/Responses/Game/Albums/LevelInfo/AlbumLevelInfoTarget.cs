using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfoTarget
{
    public AlbumLevelInfoTarget(GameLevel level, GameUser user)
    {
        Id = IdFormatter.FormatLevelId(level.Id);
        Type = GameContentType.level.ToString();
        Completed = level.UsersWhoHaveCompletedLevel.Contains(user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("completed")] public bool Completed { get; set; }
}
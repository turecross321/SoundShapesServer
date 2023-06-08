using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfoTargetResponse
{
    public AlbumLevelInfoTargetResponse(GameLevel level, GameUser user)
    {
        Id = IdHelper.FormatLevelId(level.Id);
        Type = ContentHelper.GetContentTypeString(GameContentType.Level);
        Completed = level.UniqueCompletions.Contains(user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("completed")] public bool Completed { get; set; }
}
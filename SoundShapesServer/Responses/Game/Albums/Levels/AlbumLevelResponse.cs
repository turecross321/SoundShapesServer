using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelResponse
{
    public AlbumLevelResponse(GameAlbum album, GameLevel level, GameUser user)
    {
        Id = IdFormatter.FormatAlbumLinkId(album.Id, level.Id);
        Type = GameContentType.link.ToString();
        Timestamp = level.ModificationDate.ToUnixTimeMilliseconds();
        Target = new AlbumLevelTarget(level, user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.link.ToString();
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public AlbumLevelTarget Target { get; set; }
}
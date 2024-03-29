using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelResponse : IResponse
{
    public AlbumLevelResponse(GameAlbum album, GameLevel level, GameUser user)
    {
        Id = IdHelper.FormatAlbumLinkId(album.Id, level.Id);
        Timestamp = level.ModificationDate.ToUnixTimeMilliseconds();
        Target = new LevelTargetResponse(level, user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Link);
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public LevelTargetResponse Target { get; set; }
}
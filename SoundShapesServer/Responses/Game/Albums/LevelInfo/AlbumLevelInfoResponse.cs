using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfoResponse : IResponse
{
    public AlbumLevelInfoResponse(GameLevel level, GameAlbum album, GameUser user)
    {
        Id = IdHelper.FormatAlbumLinkId(album.Id, level.Id);
        Timestamp = level.ModificationDate.ToUnixTimeMilliseconds();
        TargetResponse = new AlbumLevelInfoTargetResponse(level, user);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ContentHelper.GetContentTypeString(GameContentType.Link);
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public AlbumLevelInfoTargetResponse TargetResponse { get; set; }
}
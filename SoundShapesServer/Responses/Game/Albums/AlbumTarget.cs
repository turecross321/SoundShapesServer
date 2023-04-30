using Newtonsoft.Json;
using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumTarget
{
    public AlbumTarget(GameAlbum album, GameSession session)
    {
        Id = IdFormatter.FormatAlbumId(album.Id);
        Type = GameContentType.album.ToString();
        Metadata = new AlbumMetadata(album, session);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.album.ToString();
    [JsonProperty("metadata")] public AlbumMetadata Metadata { get; set; }
}
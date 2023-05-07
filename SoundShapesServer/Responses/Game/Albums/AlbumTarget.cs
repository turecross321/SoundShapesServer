using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumTarget
{
    public AlbumTarget(GameAlbum album)
    {
        Id = IdFormatter.FormatAlbumId(album.Id);
        Metadata = new AlbumMetadata(album);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.album.ToString();
    [JsonProperty("metadata")] public AlbumMetadata Metadata { get; set; }
}
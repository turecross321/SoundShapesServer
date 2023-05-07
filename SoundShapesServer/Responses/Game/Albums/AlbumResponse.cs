using Newtonsoft.Json;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumResponse
{
    public AlbumResponse(GameAlbum album)
    {
        Id = album.Id;
        CreationDate = album.CreationDate.ToUnixTimeMilliseconds().ToString();
        Target = new AlbumTarget(album);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.link.ToString();
    [JsonProperty("timestamp")] public string CreationDate { get; set; }
    [JsonProperty("target")] public AlbumTarget Target { get; set; }
}
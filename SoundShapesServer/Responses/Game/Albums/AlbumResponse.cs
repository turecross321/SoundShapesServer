using Newtonsoft.Json;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumResponse
{
    public AlbumResponse(GameAlbum album, GameSession session)
    {
        Id = album.Id;
        Type = GameContentType.link.ToString();
        CreationDate = album.CreationDate.ToUnixTimeMilliseconds().ToString();
        Target = new AlbumTarget(album, session);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("timestamp")] public string CreationDate { get; set; }
    [JsonProperty("target")] public AlbumTarget Target { get; set; }
}
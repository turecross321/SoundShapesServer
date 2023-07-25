using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumResponse : IResponse
{
    public AlbumResponse(GameAlbum album)
    {
        Id = album.Id;
        CreationDate = album.CreationDate.ToUnixTimeMilliseconds().ToString();
        TargetResponse = new AlbumTargetResponse(album);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Link);
    [JsonProperty("timestamp")] public string CreationDate { get; set; }
    [JsonProperty("target")] public AlbumTargetResponse TargetResponse { get; set; }
}
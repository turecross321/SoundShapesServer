using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumTargetResponse
{
    public AlbumTargetResponse(GameAlbum album)
    {
        Id = IdFormatter.FormatAlbumId(album.Id);
        MetadataResponse = new AlbumMetadataResponse(album);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Album);
    [JsonProperty("metadata")] public AlbumMetadataResponse MetadataResponse { get; set; }
}
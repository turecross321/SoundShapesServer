using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumsWrapper
{
    public AlbumsWrapper(GameAlbum[] albums, int totalAlbums, int from, int count)
    {
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalAlbums, from, count);
        Albums = albums.Select(t => new AlbumResponse(t)).ToArray();
    }

    [JsonProperty("items")] public AlbumResponse[] Albums { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}
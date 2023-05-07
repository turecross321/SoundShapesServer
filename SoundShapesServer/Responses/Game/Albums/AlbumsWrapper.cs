using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumsWrapper
{
    public AlbumsWrapper(IQueryable<GameAlbum> albums, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(albums.Count(), from, count);
        GameAlbum[] paginatedAlbums = PaginationHelper.PaginateAlbums(albums, from, count);

        Albums = paginatedAlbums.Select(t => new AlbumResponse(t)).ToArray();
        PreviousToken = previousToken;
        NextToken = nextToken;
    }

    [JsonProperty("items")] public AlbumResponse[] Albums { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}
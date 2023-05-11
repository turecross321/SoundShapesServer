using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Api.Albums;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAlbumsWrapper
{
    public ApiAlbumsWrapper(GameAlbum[] albums, int totalAlbums)
    {
        Albums = albums.Select(album => new ApiAlbumResponse(album)).ToArray();
        Count = totalAlbums;
    }

    public ApiAlbumResponse[] Albums { get; set; }
    public int Count { get; set; }
}
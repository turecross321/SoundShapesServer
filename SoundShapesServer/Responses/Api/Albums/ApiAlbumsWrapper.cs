using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Api.Albums;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAlbumsWrapper
{
    public ApiAlbumsWrapper(IQueryable<GameAlbum> albums, int from, int count, AlbumOrderType order, bool descending)
    {
        IQueryable<GameAlbum> orderedAlbums = AlbumHelper.OrderAlbums(albums, order);
        IQueryable<GameAlbum> fullyOrderedAlbums = descending ? orderedAlbums
            .AsEnumerable()
            .Reverse()
            .AsQueryable() : orderedAlbums;
        
        GameAlbum[] paginatedAlbums = PaginationHelper.PaginateAlbums(fullyOrderedAlbums, from, count);

        Albums = paginatedAlbums.Select(album => new ApiAlbumResponse(album)).ToArray();
        Count = albums.Count();
    }

    public ApiAlbumResponse[] Albums { get; set; }
    public int Count { get; set; }
}
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Extensions.Queryable;

public static class AlbumQueryableExtensions
{
    public static IQueryable<GameAlbum> OrderAlbums(this IQueryable<GameAlbum> albums, AlbumOrderType order,
        bool descending)
    {
        return order switch
        {
            AlbumOrderType.CreationDate => albums.OrderByDynamic(a => a.CreationDate, descending),
            AlbumOrderType.ModificationDate => albums.OrderByDynamic(a => a.ModificationDate, descending),
            AlbumOrderType.Plays => albums.AsEnumerable()
                .OrderByDynamic(a => a.Levels.Sum(l => l.PlaysCount), descending).AsQueryable(),
            AlbumOrderType.UniquePlays => albums.AsEnumerable()
                .OrderByDynamic(a => a.Levels.Sum(l => l.UniquePlaysCount), descending).AsQueryable(),
            AlbumOrderType.Levels => albums.AsEnumerable().OrderByDynamic(a => a.Levels.Count, descending)
                .AsQueryable(),
            AlbumOrderType.FileSize => albums.AsEnumerable()
                .OrderByDynamic(a => a.Levels.Sum(l => l.FileSize), descending).AsQueryable(),
            AlbumOrderType.Difficulty => albums.AsEnumerable()
                .OrderByDynamic(a => a.Levels.Sum(l => l.Difficulty), descending).AsQueryable(),
            _ => albums.OrderAlbums(AlbumOrderType.CreationDate, descending)
        };
    }
}
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameAlbum CreateAlbum(ApiCreateAlbumRequest request)
    {
        GameLevel[] levels = GetLevelsWithIds(request.LevelIds.AsEnumerable()).ToArray();
        GameAlbum album = new(GenerateGuid(), request, DateTimeOffset.UtcNow, levels);

        _realm.Write(() =>
        {
            _realm.Add(album);
        });

        return album;
    }

    public void RemoveAlbum(IDataStore dataStore, GameAlbum album)
    {
        dataStore.RemoveFromStore(ResourceHelper.GetAlbumResourceKey(album.Id, AlbumResourceType.Thumbnail));
        dataStore.RemoveFromStore(ResourceHelper.GetAlbumResourceKey(album.Id, AlbumResourceType.SidePanel));
        
        _realm.Write(() =>
        {
            _realm.Remove(album);
        });
    }
    
    public GameAlbum EditAlbum(GameAlbum album, ApiCreateAlbumRequest request)
    {
        _realm.Write(() =>
        {
            album.Name = request.Name;
            album.Author = request.Author;
            album.ModificationDate = DateTimeOffset.UtcNow;
            album.LinerNotes = request.LinerNotes;
            
            GameLevel[] levels = GetLevelsWithIds(request.LevelIds.AsEnumerable()).ToArray();
            album.Levels.Clear();
            foreach (GameLevel level in levels)
            {
                album.Levels.Add(level);
            }
        });

        return album;
    }
    // TODO: Implement same ordering system as levels
    public (GameAlbum[], int) GetAlbums(AlbumOrderType order, bool descending, int from, int count)
    {
        IQueryable<GameAlbum> orderedAlbums = order switch
        {
            AlbumOrderType.CreationDate => AlbumsOrderedByCreationDate(descending),
            AlbumOrderType.ModificationDate => AlbumsOrderedByModificationDate(descending),
            AlbumOrderType.Plays => AlbumsOrderedByPlays(descending),
            AlbumOrderType.UniquePlays => AlbumsOrderedByUniquePlays(descending),
            AlbumOrderType.LevelsCount => AlbumsOrderedByLevelsCount(descending),
            AlbumOrderType.FileSize => AlbumsOrderedByFileSize(descending),
            AlbumOrderType.Difficulty => AlbumsOrderedByDifficulty(descending),
            _ => AlbumsOrderedByCreationDate(descending)
        };

        // There are no Album Filters
        GameAlbum[] paginatedAlbums = PaginationHelper.PaginateAlbums(orderedAlbums, from, count);
        return (paginatedAlbums, orderedAlbums.Count());
    }

    private IQueryable<GameAlbum> AlbumsOrderedByCreationDate(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>().OrderByDescending(a => a.CreationDate);
        return _realm.All<GameAlbum>().OrderBy(a => a.CreationDate);
    }
    private IQueryable<GameAlbum> AlbumsOrderedByModificationDate(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>().OrderByDescending(a => a.ModificationDate);
        return _realm.All<GameAlbum>().OrderBy(a => a.ModificationDate);
    }
    private IQueryable<GameAlbum> AlbumsOrderedByPlays(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.PlaysCount)).AsQueryable();
        return _realm.All<GameAlbum>().OrderBy(a=> a.Levels.Sum(l=>l.PlaysCount)).AsQueryable();
    }
    private IQueryable<GameAlbum> AlbumsOrderedByUniquePlays(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.UniquePlaysCount)).AsQueryable();
        return _realm.All<GameAlbum>().OrderBy(a=> a.Levels.Sum(l=>l.UniquePlaysCount)).AsQueryable();
    }
    private IQueryable<GameAlbum> AlbumsOrderedByLevelsCount(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderByDescending(a => a.Levels.Count)
            .AsQueryable();
        return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderBy(a => a.Levels.Count)
            .AsQueryable();
    }
    private IQueryable<GameAlbum> AlbumsOrderedByFileSize(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.FileSize)).AsQueryable();
        return _realm.All<GameAlbum>().OrderBy(a=> a.Levels.Sum(l=>l.FileSize)).AsQueryable();
    }
    private IQueryable<GameAlbum> AlbumsOrderedByDifficulty(bool descending)
    {
        if (descending) return _realm.All<GameAlbum>()
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.Difficulty)).AsQueryable();
        return _realm.All<GameAlbum>().OrderBy(a=> a.Levels.Sum(l=>l.Difficulty)).AsQueryable();
    }

    public GameAlbum? GetAlbumWithId(string id)
    {
        return _realm.All<GameAlbum>().FirstOrDefault(a => a.Id == id);
    }
}
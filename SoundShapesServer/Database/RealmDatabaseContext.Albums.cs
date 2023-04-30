using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<GameAlbum> GetAlbums()
    {
        IQueryable<GameAlbum> entries = _realm.All<GameAlbum>();

        return entries;
    }

    public GameAlbum? GetAlbumWithId(string id)
    {
        return _realm.All<GameAlbum>().FirstOrDefault(a => a.Id == id);
    }

    public IQueryable<GameLevel> AlbumLevels(GameAlbum album)
    {
        IEnumerable<GameLevel> gameLevels = album.Levels;

        return gameLevels.AsQueryable();
    }
}
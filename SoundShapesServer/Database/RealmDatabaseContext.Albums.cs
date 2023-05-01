using SoundShapesServer.Types.Albums;

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
}
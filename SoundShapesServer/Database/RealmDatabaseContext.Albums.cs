using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
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

    public void RemoveAlbum(GameAlbum album)
    {
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
            album.Artist = request.Artist;
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
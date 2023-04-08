using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public AlbumsWrapper GetAlbums(int from, int count)
    {
        IEnumerable<GameAlbum> entries = this._realm.All<GameAlbum>()
            .AsEnumerable();

        IEnumerable<GameAlbum> gameAlbums = entries.ToList();
        int totalEntries = gameAlbums.Count();

        GameAlbum[] selectedEntries = gameAlbums
            .Skip(from)
            .Take(count)
            .ToArray();

        return AlbumHelper.AlbumsToAlbumsWrapper(selectedEntries, totalEntries, from, count);
    }

    public GameAlbum? GetAlbumWithId(string id)
    {
        return this._realm.All<GameAlbum>().Where(a => a.id == id).FirstOrDefault();
    }

    public AlbumLevelsWrapper AlbumLevels(GameUser user, GameAlbum album, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable();

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return AlbumHelper.LevelsToAlbumLevelsWrapper(user, album, selectedEntries, totalEntries, from, count);
    }

    public AlbumLevelInfosWrapper GetAlbumsLevelsInfo(GameUser user, GameAlbum album, int from, int count)
    {
        IEnumerable<GameLevel> entries = this._realm.All<GameLevel>()
            .AsEnumerable();

        IEnumerable<GameLevel> gameLevels = entries.ToList();
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();
        
        return AlbumHelper.LevelsToAlbumLevelInfosWrapper(user, album, selectedEntries, totalEntries, from, count);
    }
}
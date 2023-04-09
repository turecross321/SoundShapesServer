using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public AlbumsWrapper GetAlbums(string sessionId, int from, int count)
    {
        IEnumerable<GameAlbum> entries = this._realm.All<GameAlbum>()
            .AsEnumerable();

        IEnumerable<GameAlbum> gameAlbums = entries.ToList();
        int totalEntries = gameAlbums.Count();

        GameAlbum[] selectedEntries = gameAlbums
            .Skip(from)
            .Take(count)
            .ToArray();

        return AlbumHelper.AlbumsToAlbumsWrapper(sessionId, selectedEntries, totalEntries, from, count);
    }

    public GameAlbum? GetAlbumWithId(string id)
    {
        return this._realm.All<GameAlbum>().Where(a => a.id == id).FirstOrDefault();
    }

    public AlbumLevelsWrapper AlbumLevels(GameUser user, GameAlbum album, int from, int count)
    {
        IEnumerable<GameLevel> gameLevels = album.levels;
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();

        return AlbumHelper.LevelsToAlbumLevelsWrapper(user, album, selectedEntries, totalEntries, from, count);
    }

    public AlbumLevelInfosWrapper GetAlbumsLevelsInfo(GameUser user, GameAlbum album, int from, int count)
    {
        IEnumerable<GameLevel> gameLevels = album.levels;
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();
        
        return AlbumHelper.LevelsToAlbumLevelInfosWrapper(user, album, selectedEntries, totalEntries, from, count);
    }
}
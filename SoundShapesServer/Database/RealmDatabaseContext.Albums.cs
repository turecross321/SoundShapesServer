using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Responses.Game.Albums.LevelInfo;
using SoundShapesServer.Responses.Game.Albums.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<GameAlbum> GetAlbums()
    {
        IQueryable<GameAlbum> entries = this._realm.All<GameAlbum>();

        return entries;
    }

    public GameAlbum? GetAlbumWithId(string id)
    {
        return this._realm.All<GameAlbum>().FirstOrDefault(a => a.Id == id);
    }

    public IQueryable<GameLevel> AlbumLevels(GameAlbum album)
    {
        IEnumerable<GameLevel> gameLevels = album.Levels;

        return gameLevels.AsQueryable();
    }

    public AlbumLevelInfosWrapper GetAlbumsLevelsInfo(GameUser user, GameAlbum album, int from, int count)
    {
        IEnumerable<GameLevel> gameLevels = album.Levels;
        int totalEntries = gameLevels.Count();

        GameLevel[] selectedEntries = gameLevels
            .Skip(from)
            .Take(count)
            .ToArray();
        
        return AlbumHelper.LevelsToAlbumLevelInfosWrapper(user, album, selectedEntries, totalEntries, from, count);
    }
}
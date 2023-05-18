using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Responses;
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
        RemoveAlbumResources(dataStore, album);
        
        _realm.Write(() =>
        {
            _realm.Remove(album);
        });
    }

    // These aren't database related, but idk where to put them otherwise
    public Response UploadAlbumResource(IDataStore dataStore, GameAlbum album, byte[] file, AlbumResourceType resourceType)
    {
        // Album Files should always be Images
        if (!ResourceHelper.IsByteArrayPng(file))
            return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        string key = ResourceHelper.GetAlbumResourceKey(album.Id, resourceType);
        dataStore.WriteToStore(key, file);
        
        SetAlbumFilePath(album, resourceType, key);

        return HttpStatusCode.Created;
    }

    private void SetAlbumFilePath(GameAlbum album, AlbumResourceType resourceType, string path)
    {
        switch (resourceType)
        {
            case AlbumResourceType.Thumbnail:
                album.ThumbnailFilePath = path;
                break;
            case AlbumResourceType.SidePanel:
                album.SidePanelFilePath = path;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }
    }
    private void RemoveAlbumResources(IDataStore dataStore, GameAlbum album)
    {
        if (album.ThumbnailFilePath != null) dataStore.RemoveFromStore(album.ThumbnailFilePath);
        if (album.SidePanelFilePath != null) dataStore.RemoveFromStore(album.SidePanelFilePath);
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
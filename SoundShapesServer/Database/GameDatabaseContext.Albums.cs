using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
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
    public ApiOkResponse UploadAlbumResource(IDataStore dataStore, GameAlbum album, byte[] file, AlbumResourceType resourceType)
    {
        // Album Files should always be Images
        if (!ResourceHelper.IsByteArrayPng(file))
            return ApiBadRequestError.FileIsNotPng;

        string key = ResourceHelper.GetAlbumResourceKey(album.Id, resourceType);
        dataStore.WriteToStore(key, file);
        
        SetAlbumFilePath(album, resourceType, key);

        return new ApiOkResponse();
    }

    private void SetAlbumFilePath(GameAlbum album, AlbumResourceType resourceType, string path)
    {
        _realm.Write(() =>
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
        });
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
    
    public GameAlbum? GetAlbumWithId(string id)
    {
        return _realm.All<GameAlbum>().FirstOrDefault(a => a.Id == id);
    }
    
    public (GameAlbum[], int) GetPaginatedAlbums(AlbumOrderType order, bool descending, int from, int count)
    {
        IQueryable<GameAlbum> orderedAlbums = GetAlbums(order, descending);

        GameAlbum[] paginatedAlbums = PaginationHelper.PaginateAlbums(orderedAlbums, from, count);
        return (paginatedAlbums, orderedAlbums.Count());
    }

    private IQueryable<GameAlbum> GetAlbums(AlbumOrderType order, bool descending)
    {
        IQueryable<GameAlbum> albums = _realm.All<GameAlbum>();
        IQueryable<GameAlbum> orderedAlbums = OrderAlbums(albums, order, descending);

        return orderedAlbums;
    }

    #region Album Ordering

    private static IQueryable<GameAlbum> OrderAlbums(IQueryable<GameAlbum> albums, AlbumOrderType order, bool descending)
    {
        return order switch
        {
            AlbumOrderType.CreationDate => OrderAlbumsByCreationDate(albums, descending),
            AlbumOrderType.ModificationDate => OrderAlbumsByModificationDate(albums, descending),
            AlbumOrderType.Plays => OrderAlbumsByPlays(albums, descending),
            AlbumOrderType.UniquePlays => OrderAlbumsByUniquePlays(albums, descending),
            AlbumOrderType.Levels => OrderAlbumsByLevelsCount(albums, descending),
            AlbumOrderType.FileSize => OrderAlbumsByFileSize(albums, descending),
            AlbumOrderType.Difficulty => OrderAlbumsByDifficulty(albums, descending),
            _ => OrderAlbumsByCreationDate(albums, descending)
        };
    }

    private static IQueryable<GameAlbum> OrderAlbumsByCreationDate(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums.OrderByDescending(a => a.CreationDate);
        return albums.OrderBy(a => a.CreationDate);
    }
    private static IQueryable<GameAlbum> OrderAlbumsByModificationDate(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums.OrderByDescending(a => a.ModificationDate);
        return albums.OrderBy(a => a.ModificationDate);
    }
    private static IQueryable<GameAlbum> OrderAlbumsByPlays(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.PlaysCount)).AsQueryable();
        return albums.OrderBy(a=> a.Levels.Sum(l=>l.PlaysCount)).AsQueryable();
    }
    private static IQueryable<GameAlbum> OrderAlbumsByUniquePlays(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.UniquePlaysCount)).AsQueryable();
        return albums.OrderBy(a=> a.Levels.Sum(l=>l.UniquePlaysCount)).AsQueryable();
    }
    private static IQueryable<GameAlbum> OrderAlbumsByLevelsCount(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums
            .AsEnumerable()
            .OrderByDescending(a => a.Levels.Count)
            .AsQueryable();
        return albums
            .AsEnumerable()
            .OrderBy(a => a.Levels.Count)
            .AsQueryable();
    }
    private static IQueryable<GameAlbum> OrderAlbumsByFileSize(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.FileSize)).AsQueryable();
        return albums.OrderBy(a=> a.Levels.Sum(l=>l.FileSize)).AsQueryable();
    }
    private static IQueryable<GameAlbum> OrderAlbumsByDifficulty(IQueryable<GameAlbum> albums, bool descending)
    {
        if (descending) return albums
            .AsEnumerable()
            .OrderByDescending(a=> a.Levels.Sum(l=>l.Difficulty)).AsQueryable();
        return albums.OrderBy(a=> a.Levels.Sum(l=>l.Difficulty)).AsQueryable();
    }
    
    #endregion
}
using Bunkum.Core.Storage;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameAlbum CreateAlbum(ApiCreateAlbumRequest request, GameUser user)
    {
        GameLevel[] levels = GetLevelsWithIds(request.LevelIds.AsEnumerable()).ToArray();
        
        GameAlbum album = new GameAlbum
        {
            Id = GenerateGuid(),
            Name = request.Name,
            Author = user,
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow,
            LinerNotes = request.LinerNotes,
        };

        foreach (GameLevel level in levels)
        {
            album.Levels.Add(level);
        }

        _realm.Write(() =>
        {
            _realm.Add(album);
        });
        
        CreateEvent(user, EventType.AlbumCreation, PlatformType.Unknown, EventDataType.Album, album.Id);

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
        if (!file.IsPng())
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
    
    public GameAlbum EditAlbum(GameAlbum album, ApiCreateAlbumRequest request, GameUser user)
    {
        _realm.Write(() =>
        {
            album.Name = request.Name;
            album.Author = user;
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
        return _realm.All<GameAlbum>().OrderAlbums(order, descending).Paginate(from, count);
    }
}
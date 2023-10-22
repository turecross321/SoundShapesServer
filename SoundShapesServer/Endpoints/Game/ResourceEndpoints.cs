using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Core.Storage;
using Bunkum.Listener.Protocol;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Game;

public class ResourceEndpoints : EndpointGroup
{
    [GameEndpoint("~level:{levelId}/~version:{versionId}/~content:{file}/data.get"), Authentication(false)]
    public Response GetLevelResource
        (RequestContext context, IDataStore dataStore, GameDatabaseContext database, GameUser user, string levelId, string versionId, string file)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        FileType fileType = GetFileTypeFromName(file);

        string? key = fileType switch
        {
            FileType.Level => level.LevelFilePath,
            FileType.Image => level.ThumbnailFilePath,
            FileType.Sound => level.SoundFilePath,
            FileType.Unknown => null,
            _ => null
        };
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [GameEndpoint("~album:{albumId}/~content:{resource}/data.get"), Authentication(false)]
    public Response GetAlbumResource
        (RequestContext context, IDataStore dataStore, GameDatabaseContext database, string albumId, string resource)
    {
        GameAlbum? album = database.GetAlbumWithId(albumId);
        AlbumResourceType resourceType = AlbumHelper.GetAlbumResourceTypeFromString(resource);

        string? key = resourceType switch
        {
            AlbumResourceType.Thumbnail => album?.ThumbnailFilePath,
            AlbumResourceType.SidePanel => album?.SidePanelFilePath,
            _ => null
        };

        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.Png);
    }
    
    [GameEndpoint("~news:{id}/~content:thumbnail/data.get"), Authentication(false)]
    public Response GetNewsThumbnail(RequestContext context, GameDatabaseContext database, IDataStore dataStore, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);

        string? key = newsEntry?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.Png);
    }
    
    [GameEndpoint("~communityTab:{id}/~content:thumbnail/data.get"), Authentication(false)]
    public Response GetCommunityTabThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        string? key = communityTab?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.Png);
    }
}
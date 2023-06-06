using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Endpoints.Api;

public class ApiResourceEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response GetLevelThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        string? key = level?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [ApiEndpoint("levels/id/{id}/level")]
    [Authentication(false)]
    public Response GetLevelFile(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        string? key = level?.LevelFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [ApiEndpoint("albums/id/{id}/{resource}")]
    [Authentication(false)]
    private static Response GetAlbumResource(IDataStore dataStore, GameDatabaseContext database, string id, string resource)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        AlbumResourceType resourceType = AlbumHelper.GetAlbumResourceTypeFromString(resource);
        
        string? key = resourceType switch
        {
            AlbumResourceType.Thumbnail => album?.ThumbnailFilePath,
            AlbumResourceType.SidePanel => album?.SidePanelFilePath,
            _ => null
        };

        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [ApiEndpoint("news/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response NewsThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        string? key = newsEntry?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
    
    [ApiEndpoint("communityTabs/id/{id}/thumbnail")]
    [Authentication(false)]
    public Response GetCommunityTabThumbnail(RequestContext context, IDataStore dataStore, GameDatabaseContext database, string id)
    {
        CommunityTab? communityTab = database.GetCommunityTabWithId(id);
        string? key = communityTab?.ThumbnailFilePath;
        
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;
        
        return new Response(dataStore.GetDataFromStore(key), ContentType.BinaryData);
    }
}
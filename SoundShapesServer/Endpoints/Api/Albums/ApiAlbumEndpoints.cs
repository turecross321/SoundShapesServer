using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("album/{id}")]
    [Authentication(false)]
    public ApiAlbumResponse? GetAlbum(RequestContext context, RealmDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        return album == null ? null : new ApiAlbumResponse(album);
    }

    [ApiEndpoint("albums")]
    [Authentication(false)]
    public ApiAlbumsWrapper GetAlbums(RequestContext context, RealmDatabaseContext database)
    {
        IQueryable<GameAlbum> albums = database.GetAlbums();
        
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        AlbumOrderType order = orderString switch
        {
            "creationDate" => AlbumOrderType.CreationDate,
            "modificationDate" => AlbumOrderType.ModificationDate,
            "plays" => AlbumOrderType.Plays,
            "uniquePlays" => AlbumOrderType.UniquePlays,
            "levelCount" => AlbumOrderType.LevelCount,
            "fileSize" => AlbumOrderType.FileSize,
            "difficulty" => AlbumOrderType.Difficulty,
            _ => AlbumOrderType.CreationDate
        };
        
        return new ApiAlbumsWrapper(albums, from, count, order, descending);
    }
}
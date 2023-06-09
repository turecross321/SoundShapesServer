using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/id/{id}")]
    [Authentication(false)]
    public ApiAlbumResponse? GetAlbum(RequestContext context, GameDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        return album == null ? null : new ApiAlbumResponse(album);
    }

    [ApiEndpoint("albums")]
    [Authentication(false)]
    public ApiAlbumsWrapper GetAlbums(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        AlbumOrderType order = orderString switch
        {
            "creationDate" => AlbumOrderType.CreationDate,
            "modificationDate" => AlbumOrderType.ModificationDate,
            "totalPlays" => AlbumOrderType.Plays,
            "uniquePlays" => AlbumOrderType.UniquePlays,
            "levels" => AlbumOrderType.Levels,
            "fileSize" => AlbumOrderType.FileSize,
            "difficulty" => AlbumOrderType.Difficulty,
            _ => AlbumOrderType.CreationDate
        };

        (GameAlbum[] albums, int totalAlbums) = database.GetAlbums(order, descending, from, count);
        
        return new ApiAlbumsWrapper(albums, totalAlbums);
    }

    [ApiEndpoint("albums/id/{id}/completion")]
    public ApiAlbumCompletionResponse? GetAlbumCompletion(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null) return null;
        
        int completedLevels = album.Levels.Count(level => level.UniqueCompletions.Contains(user));

        return new ApiAlbumCompletionResponse(completedLevels, album.Levels.Count);
    }
}
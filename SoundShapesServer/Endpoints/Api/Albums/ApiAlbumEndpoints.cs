using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Albums;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/id/{id}"), Authentication(false)]
    [DocSummary("Retrieves album with specified ID.")]
    public ApiAlbumResponse? GetAlbum(RequestContext context, GameDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        return album == null ? null : new ApiAlbumResponse(album);
    }

    [ApiEndpoint("albums"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists albums.")]
    public ApiListResponse<ApiAlbumResponse> GetAlbums(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);
        
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
        
        return new ApiListResponse<ApiAlbumResponse>(albums.Select(a=>new ApiAlbumResponse(a)), totalAlbums);
    }

    [ApiEndpoint("albums/id/{albumId}/relationWith/id/{userId}")]
    [DocSummary("Retrieves relation between an album and a user.")]
    public ApiAlbumCompletionResponse? GetAlbumCompletion(RequestContext context, GameDatabaseContext database, string albumId, string userId)
    {
        GameAlbum? album = database.GetAlbumWithId(albumId);
        if (album == null) return null;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;
        
        int completedLevels = album.Levels.Count(level => level.UniqueCompletions.Contains(user));

        return new ApiAlbumCompletionResponse(completedLevels, album.Levels.Count);
    }
}
using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/id/{id}"), Authentication(false)]
    [DocSummary("Retrieves album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    public ApiResponse<ApiAlbumResponse> GetAlbum(RequestContext context, GameDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;
        
        return new ApiAlbumResponse(album);
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

        (GameAlbum[] albums, int totalAlbums) = database.GetPaginatedAlbums(order, descending, from, count);
        
        return new ApiListResponse<ApiAlbumResponse>(albums.Select(a=>new ApiAlbumResponse(a)), totalAlbums);
    }

    [ApiEndpoint("albums/id/{albumId}/relationWith/id/{userId}")]
    [DocSummary("Retrieves relation between an album and a user.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    public ApiResponse<ApiAlbumCompletionResponse> GetAlbumCompletion(RequestContext context, GameDatabaseContext database, string albumId, string userId)
    {
        GameAlbum? album = database.GetAlbumWithId(albumId);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null)
            return ApiNotFoundError.UserNotFound;
        
        int completedLevels = album.Levels.Count(level => level.UniqueCompletions.Contains(user));
        return new ApiAlbumCompletionResponse(completedLevels, album.Levels.Count);
    }
}
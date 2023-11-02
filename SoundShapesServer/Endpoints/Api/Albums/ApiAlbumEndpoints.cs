using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Albums;

public class ApiAlbumEndpoints : EndpointGroup
{
    [ApiEndpoint("albums/id/{id}")]
    [Authentication(false)]
    [DocSummary("Retrieves album with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    public ApiResponse<ApiAlbumResponse> GetAlbum(RequestContext context, GameDatabaseContext database, string id)
    {
        GameAlbum? album = database.GetAlbumWithId(id);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;

        return ApiAlbumResponse.FromOld(album);
    }

    [ApiEndpoint("albums")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesOrder<AlbumOrderType>]
    [DocSummary("Lists albums.")]
    public ApiListResponse<ApiAlbumResponse> GetAlbums(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        AlbumOrderType order = context.GetOrderType<AlbumOrderType>() ?? AlbumOrderType.CreationDate;

        PaginatedList<GameAlbum> albums = database.GetPaginatedAlbums(order, descending, from, count);
        return PaginatedList<GameAlbum>.FromOldList<ApiAlbumResponse, GameAlbum>(albums);
    }

    [ApiEndpoint("albums/id/{albumId}/relationWith/id/{userId}")]
    [DocSummary("Retrieves relation between an album and a user.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.AlbumNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocRouteParam("albumId", "Album ID.")]
    [DocRouteParam("userId", "User ID.")]
    public ApiResponse<ApiAlbumCompletionResponse> GetAlbumCompletion(RequestContext context,
        GameDatabaseContext database, string albumId, string userId)
    {
        GameAlbum? album = database.GetAlbumWithId(albumId);
        if (album == null)
            return ApiNotFoundError.AlbumNotFound;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null)
            return ApiNotFoundError.UserNotFound;

        int completedLevels = album.Levels.Count(level => level.UniqueCompletions.Contains(user));
        return new ApiAlbumCompletionResponse
        {
            LevelsBeaten = completedLevels,
            TotalLevels = album.Levels.Count
        };
    }
}
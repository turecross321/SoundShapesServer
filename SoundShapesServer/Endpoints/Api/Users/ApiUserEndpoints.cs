using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}")]
    [Authentication(false)]
    [DocSummary("Retrieves user with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocRouteParam("id", "User's ID.")]
    public ApiResponse<ApiUserFullResponse> GetUserWithId(RequestContext context, GameDatabaseContext database,
        string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        if (userToCheck == null)
            return ApiNotFoundError.UserNotFound;

        return ApiUserFullResponse.FromOld(userToCheck);
    }

    [ApiEndpoint("users/username/{username}")]
    [Authentication(false)]
    [DocSummary("Retrieves user with specified username.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocRouteParam("username", "User's name.")]
    public ApiResponse<ApiUserFullResponse> GetUserWithUsername(RequestContext context, GameDatabaseContext database,
        string username)
    {
        GameUser? userToCheck = database.GetUserWithUsername(username);
        if (userToCheck == null)
            return ApiNotFoundError.UserNotFound;

        return ApiUserFullResponse.FromOld(userToCheck);
    }

    [ApiEndpoint("users")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<UserFilters>]
    [DocUsesOrder<UserOrderType>]
    [DocSummary("Lists users.")]
    public ApiListResponse<ApiUserBriefResponse> GetUsers(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = context.GetPageData();

        UserFilters filters = context.GetFilters<UserFilters>(database);
        UserOrderType order = context.GetOrderType<UserOrderType>() ?? UserOrderType.CreationDate;

        PaginatedList<GameUser> users = database.GetPaginatedUsers(order, descending, filters, from, count);
        return PaginatedList<ApiUserBriefResponse>.SwapItems<ApiUserBriefResponse, GameUser>(users);
    }
}
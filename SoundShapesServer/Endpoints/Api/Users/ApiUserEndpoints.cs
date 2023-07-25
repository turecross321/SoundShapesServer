using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}"), Authentication(false)]
    [DocSummary("Retrieves user with specified ID.")]
    public ApiUserFullResponse? GetUserWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }
    
    [ApiEndpoint("users/username/{username}"), Authentication(false)]
    [DocSummary("Retrieves user with specified username.")]
    public ApiUserFullResponse? GetUserWithUsername(RequestContext context, GameDatabaseContext database, string username)
    {
        GameUser? userToCheck = database.GetUserWithUsername(username);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }

    [ApiEndpoint("users"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Lists users.")]
    public ApiListResponse<ApiUserBriefResponse> GetUsers(RequestContext context, GameDatabaseContext database)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        UserFilters filters = UserHelper.GetUserFilters(context, database);
        UserOrderType order = UserHelper.GetUserOrderType(context);

        (GameUser[] users, int totalUsers) = database.GetUsers(order, descending, filters, from, count);
        return new ApiListResponse<ApiUserBriefResponse>(users.Select(u=>new ApiUserBriefResponse(u)), totalUsers);
    }
}
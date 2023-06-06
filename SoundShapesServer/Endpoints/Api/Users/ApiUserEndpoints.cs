using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Users;

public class ApiUserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}")]
    [Authentication(false)]
    public ApiUserFullResponse? GetUserWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? userToCheck = database.GetUserWithId(id);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }
    
    [ApiEndpoint("users/username/{username}")]
    [Authentication(false)]
    public ApiUserFullResponse? GetUserWithUsername(RequestContext context, GameDatabaseContext database, string username)
    {
        GameUser? userToCheck = database.GetUserWithUsername(username);
        return userToCheck == null ? null : new ApiUserFullResponse(userToCheck);
    }

    [ApiEndpoint("users")]
    [Authentication(false)]
    public ApiUsersWrapper GetUsers(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");

        UserFilters filters = UserHelper.GetUserFilters(context, database);
        UserOrderType order = UserHelper.GetUserOrderType(context);

        (GameUser[] users, int totalUsers) = database.GetUsers(order, descending, filters, from, count);
        return new ApiUsersWrapper(users, totalUsers);
    }
}
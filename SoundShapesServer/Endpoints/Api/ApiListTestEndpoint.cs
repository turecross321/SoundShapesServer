using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class ApiListTestEndpoint : EndpointGroup
{
    [DocApiUsesPageData]
    [DocApiListResponse(typeof(ApiMinimalUserResponse))]
    [ApiEndpoint("users")]
    public ApiListResponse<ApiMinimalUserResponse> GetUsers(RequestContext context, GameDatabaseContext database)
    {
        return database.GetUsers().Select(u => ApiMinimalUserResponse.FromDb(u)).ToList();
    }
}
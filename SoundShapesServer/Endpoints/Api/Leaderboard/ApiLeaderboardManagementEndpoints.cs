using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard/id/{id}", HttpMethods.Delete)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Deletes leaderboard entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LeaderboardEntryNotFoundWhen)]
    public ApiOkResponse RemoveEntry(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        LeaderboardEntry? entry = database.GetLeaderboardEntry(id);
        if (entry == null)
            return ApiNotFoundError.LeaderboardEntryNotFound;
        
        database.RemoveLeaderboardEntry(entry);
        return new ApiOkResponse();
    }
}
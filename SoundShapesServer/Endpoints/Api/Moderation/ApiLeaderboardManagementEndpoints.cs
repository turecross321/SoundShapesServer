using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiLeaderboardManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard/{id}/remove", Method.Post)]
    public Response RemoveEntry(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        LeaderboardEntry? entry = database.GetLeaderboardWithId(id);
        if (entry == null) return HttpStatusCode.NotFound;
        
        database.RemoveLeaderboardEntry(entry);
        return HttpStatusCode.OK;
    }
}
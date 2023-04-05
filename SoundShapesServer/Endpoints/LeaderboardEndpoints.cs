using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints;

public class LeaderboardEndpoints : EndpointGroup
{
    // /otg/~identity:660c421f-e812-4a50-bdbf-082baf55f2bf/~record:%2F~level%3AWCXlogJ8.post
    [Endpoint("/otg/~identity:{userId}/~record:%2F~level%3A{arguments}", Method.Post)]
    public Response SubmitScore(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string arguments, string body)
    {
        string levelId = arguments.Split('.')[0];
        GameLevel? level = database.GetLevelWithId(levelId);

        if (level == null) return new Response(HttpStatusCode.NotFound);
        
        LeaderboardSubmissionRequest deSerializedRequest = LeaderboardHelper.DeSerializeSubmission(body);

        if (!database.SubmitScore(deSerializedRequest, user, level)) return new Response(HttpStatusCode.InternalServerError);
        
        return new Response(HttpStatusCode.OK);
    }
}
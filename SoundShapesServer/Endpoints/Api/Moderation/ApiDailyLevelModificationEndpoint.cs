using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Levels;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiDailyLevelModificationEndpoint : EndpointGroup
{
    [ApiEndpoint("daily")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    public ApiDailyLevelsWrapper? GetDailyLevelObjects(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return null;
        
        IQueryable<DailyLevel> dailyLevels = database.GetDailyLevelObjects();
        
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        // normally these things would be in helpers, but I've decided against that here to avoid confusion between
        // DailyLevel objects and GameLevel objects
        
        DailyLevel[] paginatedDailyLevels = dailyLevels.AsEnumerable().Skip(from).Take(count).ToArray();

        return new ApiDailyLevelsWrapper(
            dailyLevels: paginatedDailyLevels.Select(t => new ApiDailyLevelResponse(t)).ToArray(),
            count: dailyLevels.Count());
    }
    
    [ApiEndpoint("daily/add", Method.Post)]
    public Response AddDailyLevel(RequestContext context, RealmDatabaseContext database, GameUser user, ApiAddDailyLevelRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameLevel? level = database.GetLevelWithId(body.LevelId);
        if (level == null) return HttpStatusCode.NotFound;

        database.AddDailyLevel(level, body.DateUtc);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("daily/{id}/remove", Method.Post)]
    public Response RemoveDailyLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        DailyLevel? dailyLevel = database.GetDailyLevelWithId(id);
        if (dailyLevel == null) return HttpStatusCode.NotFound;
        
        database.RemoveDailyLevel(dailyLevel);
        return HttpStatusCode.OK;
    }
}
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    public ApiLevelResponseWrapper? GetLevels(RequestContext context, RealmDatabaseContext database, GameUser? user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? byUser = context.QueryString["byUser"];
        string? likedByUser = context.QueryString["likedByUser"];
        string? inAlbum = context.QueryString["inAlbum"];
        string? inDaily = context.QueryString["inDaily"];

        IQueryable<GameLevel>? levels = database.GetLevels();
        levels = LevelHelper.FilterLevels(database, levels, byUser, likedByUser, inAlbum, inDaily);
        if (levels == null) return null;

        LevelOrderType order = orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "plays" => LevelOrderType.Plays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "relevance" => LevelOrderType.Relevance,
            _ => LevelOrderType.CreationDate
        };
        
        return new ApiLevelResponseWrapper(levels, from, count, user, order, descending);
    }

    [ApiEndpoint("level/{levelId}")]
    [Authentication(false)]
    public ApiLevelFullResponse? Level(RequestContext context, RealmDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level, user);
    }
    
    [ApiEndpoint("level/{id}/edit", Method.Post)]
    public Response EditLevel(RequestContext context, RealmDatabaseContext database, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (PermissionHelper.IsUserAdmin(user) == false)
                return HttpStatusCode.Unauthorized;
        }
        
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new Response(new ApiLevelFullResponse(publishedLevel, user), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("level/{id}/remove", Method.Post)]
    public Response RemoveLevel(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (PermissionHelper.IsUserAdmin(user) == false)
                return HttpStatusCode.Unauthorized;
        }

        database.RemoveLevel(level, dataStore);
        return HttpStatusCode.OK;
    }
}
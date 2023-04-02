using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelsEndpoints : EndpointGroup
{
    private LevelResponsesWrapper GetLevels(List<GameLevel> levels, int count, RealmDatabaseContext database)
    {
        LevelResponse[] levelResponses = new LevelResponse[Math.Min(count, levels.Count())];

        for (int i = 0; i < levels.Count; i++)
        {
            string formattedLevelId = database.FormatLevelId(levels[i].id);
            string formattedAuthorId = database.FormatUserId(levels[i].author.id);

            LevelAuthor author = new()
            {
                id = formattedAuthorId,
                type = ResponseType.identity.ToString(),
                display_name = levels[i].author.display_name
            };

            LevelResponse levelResponse = new LevelResponse()
            {
                id = formattedLevelId,
                author = author,
                latestVersion =
                    $"/~level:{levels[i].id}/~version:{levels[i].creationTime}", // TODO: IMPLEMENT THIS PROPERLY
                title = levels[i].title,
                description = levels[i].description,
                type = ResponseType.level.ToString(),
                metadata = LevelHelper.GenerateMetadataResponse(levels[i].metadata)
            };

            levelResponses[i] = levelResponse;
        }

        LevelResponsesWrapper response = new()
        {
            items = levelResponses,
            count = levelResponses.Count()
        };

        return response;
    }
    
    [Endpoint("/otg/~index:*.page", ContentType.Json)]
    [Endpoint("/otg/~index:level.page", ContentType.Json)]
    public LevelResponsesWrapper LevelsEndpoint(RequestContext context, RealmDatabaseContext database)
    {
        var category = context.QueryString["search"];
        var order = context.QueryString["order"];
        var type = context.QueryString["type"];
        var from = context.QueryString["from"];
        var query = context.QueryString["query"];
        int count = int.Parse(context.QueryString["count"]);
        var decorated = context.QueryString["decorated"];

        List<GameLevel> levels = new List<GameLevel>();
        
        if (query != null && query.Contains("author.id:")) // Levels by player
            levels = LevelsByUser(query, count, database);

        return GetLevels(levels, count, database);
    }

    [Endpoint("/otg/~identity:{userId}/~queued:*.page", ContentType.Json)]
    public LevelResponsesWrapper QueuedAndLiked(RequestContext context, RealmDatabaseContext database, string userId)
    {
        int count = int.Parse(context.QueryString["count"]);
        var decorate = context.QueryString["decorate"];

        GameUser user = database.GetUserWithId(userId);

        List<GameLevel> levels = database.GetUsersLikedLevels(user);
        
        return GetLevels(levels, count, database);
    }
    private List<GameLevel> LevelsByUser(string query, int count, RealmDatabaseContext database)
    {
        string id = query.Split(":")[2];

        GameUser user = database.GetUserWithId(id);

        var levels = database.GetLevelsPublishedByUser(user, count);

        return levels;
    }
}
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelEndpoints : EndpointGroup
{
    // /otg/~index:level.page?search=newest&type=level&count=9
    [Endpoint("/otg/~index:level.page", ContentType.Json)]
    [Authentication(true)]
    public LevelListResponse GetLevels(RequestContext context, RealmDatabaseContext database)
    {
        return null; // TODO: THE RESULTS NEED TO BE RETURNED IN CHUNKS FOR THE GAME TO NOT CRASH

        var category = context.QueryString["search"];
        var order = context.QueryString["order"];
        var type = context.QueryString["type"];
        var from = context.QueryString["from"];
        var query = context.QueryString["query"];
        var count = context.QueryString["count"];
        var decorated = context.QueryString["decorated"];

        if (true) // Greatest Hits
        {
            var levels = database.GetAllLevels();

            List<LevelResponse> levelResponses = new List<LevelResponse>();

            for (int i = 0; i < levels.Count; i++)
            {
                string formattedLevelId = database.FormatLevelId(levels[i].id);
                string formattedAuthorId = database.FormatUserId(levels[i].author.id);

                LevelAuthor author = new()
                {
                    id = formattedAuthorId,
                    type = ItemType.identity.ToString(),
                    display_name = levels[i].author.display_name
                };

                LevelMetadataResponse metadata = new()
                {
                    plays_of_recent_count = 0,
                    plays_of_ever_average_deaths = 12.7f,
                    difficulty = 5,
                    golds_today_count = 1,
                    likes_of_ever_count = 43,
                    timestamp = 1455749749571,
                    unique_plays_ever_total_golds = 10,
                    created = "2014-09-14T04:16:42-0400",
                    unique_plays_ever_average_golds = 0.01821493624f,
                    unique_plays_ever_average_completes = 0.3169f,
                    plays_of_ever_average_tokens = 28.863f,
                    unique_plays_ever_total_completes = 174,
                    plays_of_recent_count_trend = 1,
                    unique_plays_ever_count = 549,
                    image =
                        "https://media.discordapp.net/attachments/994205645244989500/1090675406958510180/h5ptpbzg.png?width=352&height=198",
                    modified = "2016-02-17T17:55:49-0500",
                    plays_of_ever_count = 695,
                    plays_of_ever_average_time = 266554.6935251799f,
                    plays_of_ever_total_time = 1.86266612E8,
                    plays_of_ever_total_tokens = 20060,
                    likes_of_this_week_count = 3,
                    plays_of_ever_total_deaths = 8857,
                    queasy3 = 1535846400,
                    displayName = "kill me",
                    sce_np_language = 1
                };

                levelResponses.Add(new LevelResponse()
                {
                    id = formattedLevelId,
                    author = author,
                    latestVersion = $"/~level:{levels[i].id}/~version:{levels[i].creationTime}", // IMPLEMENT THIS PROPERLY
                    title = levels[i].title,
                    description = levels[i].description,
                    type = ItemType.level.ToString(),
                    metadata = metadata
                });
            }

            return new LevelListResponse()
            {
                items = levelResponses,
                count = levelResponses.Count,
                nextToken = levelResponses.Count // FIX THIS
            };
        }

        return null;

    } // "/otg/~index:level.page?search={searchQuery}&type={type}&from={from}&count={count}
}
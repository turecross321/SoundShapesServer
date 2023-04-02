using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public class LevelHelper
{
    public static LevelMetadataResponse GenerateMetadataResponse(LevelMetadata metadata)
    {
        LevelMetadataResponse response = new LevelMetadataResponse()
        {
            plays_of_ever_average_tokens = metadata.plays_of_ever_average_tokens.ToString(),
            unique_plays_ever_total_completes = metadata.unique_plays_ever_total_completes.ToString(),
            plays_of_recent_count = metadata.plays_of_recent_count.ToString(),
            plays_of_recent_count_trend = metadata.plays_of_recent_count_trend.ToString(),
            plays_of_ever_average_deaths = metadata.plays_of_recent_count_trend.ToString(),
            image = metadata.image,
            unique_plays_ever_count = metadata.unique_plays_ever_count.ToString(),
            difficulty = metadata.difficulty.ToString(),
            golds_today_count = metadata.golds_today_count.ToString(),
            modified = metadata.modified.ToString(),
            timestamp = metadata.timestamp.ToString(),
            plays_of_ever_count = metadata.plays_of_ever_count.ToString(),
            plays_of_ever_average_time = metadata.plays_of_ever_average_time.ToString(),
            unique_plays_ever_total_golds = metadata.unique_plays_ever_total_golds.ToString(),
            created = metadata.created.ToString(),
            unique_plays_ever_average_golds = metadata.unique_plays_ever_average_golds.ToString(),
            plays_of_ever_total_time = metadata.plays_of_ever_total_time.ToString(),
            plays_of_ever_total_tokens = metadata.plays_of_ever_total_tokens.ToString(),
            plays_of_ever_total_deaths = metadata.plays_of_ever_total_deaths.ToString(),
            displayName = metadata.displayName,
            sce_np_language = metadata.sce_np_language.ToString(),
            unique_plays_ever_average_completes = metadata.unique_plays_ever_average_completes.ToString()
        };

        return response;
    }
}
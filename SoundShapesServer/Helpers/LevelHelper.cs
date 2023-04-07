using System.Diagnostics;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    private static LevelMetadataResponse GenerateMetadataResponse(GameLevel level)
    {
        float difficulty;

        if (level.deaths > 0)
        {
            float rate = level.deaths / level.plays;
            difficulty = Math.Clamp(rate, 0, 5);
        }
        else difficulty = 0;
        
        LevelMetadataResponse response = new LevelMetadataResponse()
        {
            displayName = level.title,
            
            unique_plays_ever_count = level.uniquePlays.Count.ToString(),
            difficulty = difficulty.ToString(),
            timestamp = level.modified.ToUnixTimeMilliseconds().ToString(),
            plays_of_ever_count = level.plays.ToString(),
            sce_np_language = level.scp_np_language.ToString(),
            likes_of_ever_count = level.likes.Count().ToString()
        };

        return response;
    }

    public static LevelResponse ConvertGameLevelToLevelResponse(GameLevel level)
    {
        string formattedLevelId = IdFormatter.FormatLevelId(level.id);

        LevelResponse levelResponse = new LevelResponse()
        {
            id = formattedLevelId,
            author = UserHelper.GetUserResponseFromGameUser(level.author),
            latestVersion = IdFormatter.FormatLevelIdAndVersion(level.id, level.created.ToUnixTimeMilliseconds()),
            title = level.title,
            description = level.description,
            type = ResponseType.level.ToString(),
            metadata = GenerateMetadataResponse(level)
        };

        return levelResponse;
    }
}
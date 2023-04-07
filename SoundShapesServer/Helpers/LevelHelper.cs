using System.Diagnostics;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
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

    public static LevelResponsesWrapper ConvertGameLevelArrayToLevelResponseWrapper(GameLevel[] levels, GameUser user, int totalEntries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);
        
        LevelResponse[] levelResponses = new LevelResponse[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            levelResponses[i] = ConvertGameLevelToLevelResponse(levels[i], user);
        }

        LevelResponsesWrapper response = new()
        {
            items = levelResponses,
            count = levelResponses.Length,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return response;
    }
    
    public static LevelResponse ConvertGameLevelToLevelResponse(GameLevel level, GameUser user)
    {
        string formattedLevelId = IdFormatter.FormatLevelId(level.id);

        LevelResponse levelResponse = new LevelResponse()
        {
            id = formattedLevelId,
            author = UserHelper.ConvertGameUserToUserResponse(level.author),
            latestVersion = IdFormatter.FormatLevelIdAndVersion(level.id, level.created.ToUnixTimeMilliseconds()),
            title = level.title,
            description = level.description,
            type = ResponseType.level.ToString(),
            completed = level.completionists.Contains(user),
            metadata = GenerateMetadataResponse(level)
        };

        return levelResponse;
    }
}
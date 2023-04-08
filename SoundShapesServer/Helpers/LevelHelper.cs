using System.Data;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    public static LevelMetadataResponse GenerateMetadataResponse(GameLevel level)
    {
        float difficulty;

        if (level.deaths > 0)
        {
            float rate = level.deaths / level.plays;
            difficulty = Math.Clamp(rate, 1, 5);
        }
        else difficulty = 0;
        
        LevelMetadataResponse response = new ()
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

    public static LevelsWrapper LevelsToLevelsWrapper(GameLevel[] levels, GameUser user, int totalEntries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        List<LevelResponse> levelResponses = new ();

        for (int i = 0; i < levels.Length; i++)
        {
            LevelResponse? levelResponse = LevelToLevelResponse(levels[i], user);
            if (levelResponse != null) levelResponses.Add(levelResponse);
        }

        LevelsWrapper response = new()
        {
            items = levelResponses.ToArray(),
            count = levelResponses.Count,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return response;
    }
    
    public static LevelResponse? LevelToLevelResponse(GameLevel? level, GameUser user)
    {
        if (level == null) return null;
        
        string formattedLevelId = IdFormatter.FormatLevelId(level.id);

        LevelResponse levelResponse = new LevelResponse()
        {
            id = formattedLevelId,
            author = UserHelper.UserToUserResponse(level.author),
            latestVersion = IdFormatter.FormatLevelIdAndVersion(level.id, level.modified.ToUnixTimeMilliseconds()),
            title = level.title,
            description = level.description,
            type = ResponseType.level.ToString(),
            completed = level.completionists.Contains(user),
            metadata = GenerateMetadataResponse(level)
        };

        return levelResponse;
    }
}
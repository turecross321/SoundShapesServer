using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LevelHelper
{
    private const string LevelIdCharacters = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int LevelIdLength = 8;
    
    public static string GenerateLevelId(RealmDatabaseContext database)
    {
        Random r = new Random();
        string levelId = "";
        for (int i = 0; i < LevelIdLength; i++)
        {
            levelId += LevelIdCharacters[r.Next(LevelIdCharacters.Length - 1)];
        }

        if (database.GetLevelWithId(levelId) == null) return levelId; // Return if LevelId has not been used before
        return GenerateLevelId(database); // Generate new LevelId if it already exists
    }

    public static LevelPublishResponse GeneratePublishResponse(GameLevel level)
    {
        return new () {
            Id = IdFormatter.FormatLevelPublishId(level.Id, level.CreationDate.ToUnixTimeMilliseconds()),
            Type = ResponseType.upload.ToString(),
            Author = new()
            {
                Id = IdFormatter.FormatUserId(level.Author.Id),
                DisplayName = level.Author.Username
            },
            Title = level.Name,
            Dependencies = new List<string>(),
            Visibility = "EVERYONE",
            Description = level.Description,
            ExtraData = new ExtraDataResponse() { Language = level.Language },
            Parent = new()
            {
                Id = IdFormatter.FormatLevelId(level.Id),
                Type = ResponseType.level.ToString()
            },
            CreationDate = level.CreationDate.ToUnixTimeMilliseconds()
        };   
    }
    public static LevelMetadataResponse GenerateMetadataResponse(GameLevel level)
    {
        float difficulty;

        if (level.Deaths > 0)
        {
            float rate = level.Deaths / level.Plays;
            difficulty = Math.Clamp(rate, 1, 5);
        }
        else difficulty = 0;
        
        LevelMetadataResponse response = new ()
        {
            Name = level.Name,
            
            UniquePlaysCount = level.UniquePlays.Count.ToString(),
            Difficulty = difficulty.ToString(),
            Timestamp = level.ModificationDate.ToUnixTimeMilliseconds().ToString(),
            TotalPlaysCount = level.Plays.ToString(),
            Language = level.Language.ToString(),
            LikesCount = level.Likes.Count().ToString()
        };

        return response;
    }

    public static LevelsWrapper LevelsToLevelsWrapper(IQueryable<GameLevel> levels, GameUser user, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(levels.Count(), from, count);
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(levels, from, count);

        List<LevelResponse> levelResponses = new ();

        for (int i = 0; i < paginatedLevels.Length; i++)
        {
            LevelResponse? levelResponse = LevelToLevelResponse(paginatedLevels[i], user);
            if (levelResponse != null) levelResponses.Add(levelResponse);
        }

        LevelsWrapper response = new()
        {
            Levels = levelResponses.ToArray(),
            LevelCount = levelResponses.Count,
            NextToken = nextToken,
            PreviousToken = previousToken
        };

        return response;
    }
    
    public static LevelResponse? LevelToLevelResponse(GameLevel level, GameUser user)
    {
        string formattedLevelId = IdFormatter.FormatLevelId(level.Id);

        LevelResponse levelResponse = new LevelResponse()
        {
            Id = formattedLevelId,
            Author = UserHelper.UserToUserResponse(level.Author),
            LatestVersion = IdFormatter.FormatLevelIdAndVersion(level.Id, level.ModificationDate.ToUnixTimeMilliseconds()),
            Title = level.Name,
            Description = level.Description,
            Type = ResponseType.level.ToString(),
            Completed = level.UsersWhoHaveCompletedLevel.Contains(user),
            Metadata = GenerateMetadataResponse(level)
        };

        return levelResponse;
    }
    
    // API

    public static ApiLevelResponseWrapper LevelsToApiLevelResponseWrapper(IQueryable<GameLevel> levels, int from, int count, GameUser? user)
    {
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(levels, from, count);
        
        ApiLevelResponse[] levelResponses = new ApiLevelResponse[paginatedLevels.Length];
        
        for (int i = 0; i < paginatedLevels.Length; i++)
        {
            levelResponses[i] = LevelToApiLevelResponse(paginatedLevels[i], user);
        }

        return new ApiLevelResponseWrapper()
        {
            Levels = levelResponses,
            Count = levels.Count()
        };
    }

    public static ApiLevelResponse LevelToApiLevelResponse(GameLevel level, GameUser? user)
    {
        bool? completed = null;
        if (user != null) completed = level.UsersWhoHaveCompletedLevel.Contains(user);
        
        return new ApiLevelResponse()
        {
            Id = level.Id,
            Name = level.Name,
            AuthorId = level.Author.Id,
            AuthorName = level.Author.Username,
            TotalPlays = level.Plays,
            UniquePlays = level.UniquePlays.Count,
            Likes = level.Likes.Count(),
            CompletedByYou = completed
        };
    }
}
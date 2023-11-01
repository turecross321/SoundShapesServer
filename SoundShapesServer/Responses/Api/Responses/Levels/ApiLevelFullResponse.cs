using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiLevelFullResponse : IApiResponse, IDataConvertableFrom<ApiLevelFullResponse, GameLevel>
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }
    public required int Language { get; set; }
    public required LevelVisibility Visibility { get; set; }
    public required PlatformType UploadPlatform { get; set; }
    public required ApiLevelAnalysisResponse Analysis { get; set; }
    public required int TotalPlays { get; set; }
    public required int UniquePlays { get; set; }
    public required int TotalCompletions { get; set; }
    public required int UniqueCompletions { get; set; }
    public required int Likes { get; set; }
    public required int Queues { get; set; }
    public required int TotalDeaths { get; set; }
    public required long TotalPlayTime { get; set; }
    public required float Difficulty { get; set; }
    public required IEnumerable<ApiAlbumResponse> Albums { get; set; }
    public required IEnumerable<ApiDailyLevelResponse> DailyLevels { get; set; }

    public static ApiLevelFullResponse FromOld(GameLevel old)
    {
        return new ApiLevelFullResponse
        {
            Id = old.Id,
            Name = old.Name,
            Author = ApiUserBriefResponse.FromOld(old.Author),
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            Language = old.Language,
            Visibility = old.Visibility,
            UploadPlatform = old.UploadPlatform,
            Analysis = ApiLevelAnalysisResponse.FromOld(old),
            TotalPlays = old.PlaysCount,
            UniquePlays = old.UniquePlaysCount,
            TotalCompletions = old.CompletionCount,
            UniqueCompletions = old.UniqueCompletions.Count,
            Likes = old.Likes.Count(),
            Queues = old.Queues.Count(),
            TotalDeaths = old.TotalDeaths,
            TotalPlayTime = old.TotalPlayTime,
            Difficulty = old.Difficulty,
            Albums = ApiAlbumResponse.FromOldList(old.Albums),
            DailyLevels = ApiDailyLevelResponse.FromOldList(old.DailyLevels)
        };
    }

    public static IEnumerable<ApiLevelFullResponse> FromOldList(IEnumerable<GameLevel> oldList)
    {
        return oldList.Select(FromOld);
    }
}
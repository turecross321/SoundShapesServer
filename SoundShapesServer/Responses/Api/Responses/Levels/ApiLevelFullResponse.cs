using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiLevelFullResponse : IApiResponse
{
    public ApiLevelFullResponse(GameLevel level)
    {
        Id = level.Id;
        Name = level.Name;
        Author = new ApiUserBriefResponse(level.Author);
        CreationDate = level.CreationDate;
        ModificationDate = level.ModificationDate;
        Language = level.Language;
        Visibility = level.Visibility;
        UploadPlatform = level.UploadPlatform;
        Analysis = new ApiLevelAnalysisResponse(level);
        TotalPlays = level.PlaysCount;
        UniquePlays = level.UniquePlaysCount;
        TotalCompletions = level.CompletionCount;
        UniqueCompletions = level.UniqueCompletions.Count;
        Likes = level.Likes.Count();
        Queues = level.Queues.Count();
        TotalDeaths = level.TotalDeaths;
        TotalPlayTime = level.TotalPlayTime;
        Difficulty = level.Difficulty;
        Albums = level.Albums.AsEnumerable().Select(a => new ApiAlbumResponse(a)).ToArray();
        DailyLevels = level.DailyLevels.AsEnumerable().Select(a => new ApiDailyLevelResponse(a)).ToArray();
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public int Language { get; set; }
    public LevelVisibility Visibility { get; set; }
    public PlatformType UploadPlatform { get; set; }
    public ApiLevelAnalysisResponse Analysis { get; set; }
    public int TotalPlays { get; set; }
    public int UniquePlays { get; set; }
    public int TotalCompletions { get; set; }
    public int UniqueCompletions { get; set; }
    public int Likes { get; set; }
    public int Queues { get; set; }
    public int TotalDeaths { get; set; }
    public long TotalPlayTime { get; set; }
    public float Difficulty { get; set; }
    public ApiAlbumResponse[] Albums { get; set; }
    public ApiDailyLevelResponse[] DailyLevels { get; set; }
}
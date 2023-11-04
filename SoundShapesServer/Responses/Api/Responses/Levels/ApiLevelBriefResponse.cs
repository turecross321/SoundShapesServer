using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiLevelBriefResponse : IApiResponse, IDataConvertableFrom<ApiLevelBriefResponse, GameLevel>
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }
    public required int TotalPlays { get; set; }
    public required int UniquePlays { get; set; }
    public required int Likes { get; set; }
    public required int Queues { get; set; }
    public required float Difficulty { get; set; }
    public required LevelVisibility Visibility { get; set; }
    public required bool CampaignLevel { get; set; }

    public static ApiLevelBriefResponse FromOld(GameLevel old)
    {
        return new ApiLevelBriefResponse
        {
            Id = old.Id,
            Name = old.Name,
            Author = ApiUserBriefResponse.FromOld(old.Author),
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            TotalPlays = old.PlaysCount,
            UniquePlays = old.UniquePlaysCount,
            Likes = old.Likes.Count(),
            Queues = old.Queues.Count(),
            Difficulty = old.Difficulty,
            Visibility = old.Visibility,
            CampaignLevel = LevelHelper.OfflineLevelIds.Contains(old.Id)
        };
    }

    public static IEnumerable<ApiLevelBriefResponse> FromOldList(IEnumerable<GameLevel> oldList)
    {
        return oldList.Select(FromOld);
    }
}
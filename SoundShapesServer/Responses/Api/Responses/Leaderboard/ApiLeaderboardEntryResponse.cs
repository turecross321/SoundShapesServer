using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Api.Responses.Leaderboard;

public class ApiLeaderboardEntryResponse : IApiResponse
{
    public required string Id { get; set; }
    public required int Position { get; set; }
    public required bool Obsolete { get; set; }
    public required ApiUserBriefResponse User { get; set; }
    public required long Score { get; set; }
    public required long PlayTime { get; set; }
    public required int Notes { get; set; }
    public required int Deaths { get; set; }
    public required bool Completed { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required PlatformType PlatformType { get; set; }

    public static ApiLeaderboardEntryResponse FromOld(
        LeaderboardEntry entry, LeaderboardOrderType order, LeaderboardFilters filters)
    {
        return new ApiLeaderboardEntryResponse
        {
            Id = entry.Id.ToString()!,
            Position = entry.GetPosition(order, filters),
            Obsolete = entry.Obsolete(),
            User = ApiUserBriefResponse.FromOld(entry.User),
            Score = entry.Score,
            PlayTime = entry.PlayTime,
            Notes = entry.Notes,
            Deaths = entry.Deaths,
            Completed = entry.Completed,
            CreationDate = entry.CreationDate,
            PlatformType = entry.PlatformType
        };
    }


    public static IEnumerable<ApiLeaderboardEntryResponse> FromOldList(
        IEnumerable<LeaderboardEntry> entries, LeaderboardOrderType order, LeaderboardFilters filters)
    {
        return entries.Select(e => FromOld(e, order, filters));
    }
}
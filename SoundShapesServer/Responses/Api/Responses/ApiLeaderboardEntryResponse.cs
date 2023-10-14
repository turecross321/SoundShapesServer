using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiLeaderboardEntryResponse : IApiResponse
{
    public ApiLeaderboardEntryResponse(LeaderboardEntry entry, int position)
    {
        Id = entry.Id;
        Position = position;
        User = new ApiUserBriefResponse(entry.User);
        Score = entry.Score;
        PlayTime = entry.PlayTime;
        Notes = entry.Notes;
        Deaths = entry.Deaths;
        Completed = entry.Completed;
        CreationDate = entry.CreationDate.ToUnixTimeSeconds();
        PlatformType = entry.PlatformType;
    }

    public string Id { get; set; }
    public int Position { get; set; }
    public ApiUserBriefResponse User { get; set; }
    public long Score { get; set; }
    public long PlayTime { get; set; }
    public int Notes { get; set; }
    public int Deaths { get; set; }
    public bool Completed { get; set; }
    public long CreationDate { get; set; }
    public PlatformType PlatformType { get; set; }
}
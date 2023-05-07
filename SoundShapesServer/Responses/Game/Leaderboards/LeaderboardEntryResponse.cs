using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntryResponse
{
    public LeaderboardEntryResponse(LeaderboardEntry entry, int position)
    {
        Position = position;
        Entrant = new UserResponse(entry.User);
        Score = entry.Score;
    }

    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("entrant")] public UserResponse? Entrant { get; set; }
    [JsonProperty("score")] public long Score { get; set; }
}
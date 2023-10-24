using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types.Leaderboard;

namespace SoundShapesServer.Responses.Game;

public class LeaderboardEntryResponse : IResponse
{
    public LeaderboardEntryResponse(LeaderboardEntry entry, LeaderboardOrderType order, LeaderboardFilters filters)
    {
        Position = entry.GetPosition(order, filters) + 1;
        Entrant = new UserTargetResponse(entry.User);
        Score = entry.Score;
    }

    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("entrant")] public UserTargetResponse? Entrant { get; set; }
    [JsonProperty("score")] public long Score { get; set; }
}
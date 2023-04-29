using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;

namespace SoundShapesServer.Responses.Game.Leaderboards;

public class LeaderboardEntryResponse
{
    [JsonProperty("position")] public int Position { get; set; }
    [JsonProperty("entrant")] public UserResponse? Entrant { get; set; }
    [JsonProperty("score")] public long Score { get; set; }
}
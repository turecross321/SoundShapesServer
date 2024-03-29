using System.Globalization;
using Newtonsoft.Json;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelMetadataResponse : IResponse
{
    public LevelMetadataResponse(GameLevel level)
    {
        Name = level.Name;
        UniquePlaysCount = level.UniquePlaysCount.ToString();
        Difficulty = level.Difficulty.ToString(CultureInfo.InvariantCulture);
        Timestamp = level.ModificationDate.ToUnixTimeMilliseconds().ToString();
        TotalPlays = level.PlaysCount.ToString();
        Language = level.Language.ToString();
        Likes = level.Likes.Count().ToString();
    }

    public LevelMetadataResponse(){}
    public static LevelMetadataResponse PrivateLevel()
    {
        return new LevelMetadataResponse
        {
            Name = "Private Level"
        };
    }
    
    [JsonProperty("unique_plays_ever_count")] public string UniquePlaysCount { get; set; } = "0";
    [JsonProperty("difficulty")] public string Difficulty { get; set; } = "0";
    [JsonProperty("timestamp")] public string Timestamp { get; set; } = "0";
    [JsonProperty("plays_of_ever_count")] public string TotalPlays { get; set; } = "0";
    [JsonProperty("displayName")] public string Name { get; set; } = "";
    [JsonProperty("sce_np_language")] public string Language { get; set; } = "0";
    [JsonProperty("likes_of_ever_count")] public string Likes { get; set; } = "0";
}
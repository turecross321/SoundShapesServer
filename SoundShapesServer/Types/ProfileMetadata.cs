using Newtonsoft.Json;
using Realms;

namespace SoundShapesServer.Types;

[JsonObject(MemberSerialization.OptIn)]
public class ProfileMetadata : EmbeddedObject // this is the most unhinged shit i have ever fucking seen
{
    [JsonProperty] public string displayName { get; set; }
    [JsonProperty] public int follows_of_ever_count { get; set; } // My fans
    [JsonProperty] public int levels_by_ever_count { get; set; } // Amount of published levels
    [JsonProperty] public int follows_by_ever_count { get; set; } // Following
    [JsonProperty] public int likes_by_ever_count { get; set; } // Liked and Queued Levels
    
    // The server used to provide these, but the game doesn't actually do anything with them. 
    
    /*public float unique_plays_ever_total_completes { get; set; }
    public double plays_by_ever_total_time { get; set; }
    public int unique_plays_ever_count { get; set; }
    public float plays_by_ever_average_time { get; set; }
    public int golds_today_count { get; set; }
    public string featuredLevel { get; set; }
    public float plays_by_ever_average_tokens { get; set; }
    public string  lastLogin { get; set; }
    public float plays_by_ever_total_tokens { get; set; }
    public float plays_by_ever_total_deaths { get; set; }
    public float unique_plays_ever_total_golds { get; set; }
    public int plays_by_ever_count { get; set; }
    public float unique_plays_ever_average_golds { get; set; }
    public float plays_by_ever_average_deaths { get; set; }
    public float unique_plays_ever_average_completes { get; set; }*/
}
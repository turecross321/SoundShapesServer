using Realms;

namespace SoundShapesServer.Types;

public class Metadata : EmbeddedObject
{
    public int plays_of_recent_count { get; set; }
    public float plays_of_ever_average_deaths { get; set; }
    public int difficulty { get; set; }
    public int golds_today_count { get; set; }
    public int likes_of_ever_count { get; set; }
    public long timestamp { get; set; }
    public float unique_plays_ever_total_golds { get; set; }
    public string created { get; set; }
    public float unique_plays_ever_average_golds { get; set; }
    public float unique_plays_ever_average_completes { get; set; }
    public float plays_of_ever_average_tokens { get; set; }
    public float unique_plays_ever_total_completes { get; set; }
    public float plays_of_recent_count_trend { get; set; }
    public int unique_plays_ever_count { get; set; }
    public string image { get; set; }
    public string modified { get; set; }
    public int plays_of_ever_count { get; set; }
    public float plays_of_ever_average_time { get; set; }
    public double plays_of_ever_total_time { get; set; }
    public float plays_of_ever_total_tokens { get; set; }
    public int likes_of_this_week_count { get; set; }
    public float plays_of_ever_total_deaths { get; set; }
    public int queasy3 { get; set; } 
    public string displayName { get; set; }
    public int sce_np_language { get; set; }
}
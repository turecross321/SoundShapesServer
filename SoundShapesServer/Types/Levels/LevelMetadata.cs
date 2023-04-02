using Newtonsoft.Json;
using Realms;

namespace SoundShapesServer.Types.Levels;

public class LevelMetadata : EmbeddedObject
{
    public float plays_of_ever_average_tokens { get; set; }   
    public float unique_plays_ever_total_completes { get; set; }  
    public int plays_of_recent_count { get; set; }  
    public float plays_of_recent_count_trend { get; set; }  
    public float plays_of_ever_average_deaths { get; set; }  
    public string image { get; set; }  
    public int unique_plays_ever_count { get; set; }  
    public int difficulty { get; set; }  
    public int golds_today_count { get; set; }  
    public DateTimeOffset modified { get; set; }  
    public long timestamp { get; set; }  
    public int plays_of_ever_count { get; set; }  
    public float plays_of_ever_average_time { get; set; }  
    public float unique_plays_ever_total_golds { get; set; }  
    public DateTimeOffset created { get; set; }  
    public float unique_plays_ever_average_golds { get; set; }  
    public float plays_of_ever_total_time { get; set; }  
    public float plays_of_ever_total_tokens { get; set; }  
    public float plays_of_ever_total_deaths { get; set; }  
    public string displayName { get; set; }  
    public int sce_np_language { get; set; }  
    public float unique_plays_ever_average_completes { get; set; }  
}
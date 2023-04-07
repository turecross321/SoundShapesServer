namespace SoundShapesServer.Responses;

public class UserMetadataResponse
{
    public string displayName { get; set; }
    public int follows_of_ever_count { get; set; } // My fans
    public int levels_by_ever_count { get; set; } // Amount of published levels
    public int follows_by_ever_count { get; set; } // Following
    public int likes_by_ever_count { get; set; } // Liked and Queued Levels
}
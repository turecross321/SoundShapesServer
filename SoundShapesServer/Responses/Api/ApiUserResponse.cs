namespace SoundShapesServer.Responses.Api;

public class ApiUserResponse
{
    public string Username { get; set; }
    public bool Online { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public int LikedLevelsCount { get; set; }
    public int PublishedLevelsCount { get; set; }
}
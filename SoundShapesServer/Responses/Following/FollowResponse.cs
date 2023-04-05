using SoundShapesServer.Enums;

namespace SoundShapesServer.Responses.Following;

public class FollowResponse
{
    public string id { get; set; }
    public string type { get; set; } = ResponseType.follow.ToString();
    public UserResponse target { get; set; }
}
using Realms.Sync;
using SoundShapesServer.Enums;

namespace SoundShapesServer.Responses.Following;

public class FollowingUserResponse
{
    public string id { get; set; }
    public string type { get; } = ResponseType.follow.ToString();
    public UserResponse target { get; set; }
}
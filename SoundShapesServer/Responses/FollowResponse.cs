using SoundShapesServer.Enums;

namespace SoundShapesServer.Responses;

public class FollowResponse
{
    public string id { get; set; }
    public string type { get; set; } = ResponseType.follow.ToString();
    public RelationTarget target { get; set; }
}
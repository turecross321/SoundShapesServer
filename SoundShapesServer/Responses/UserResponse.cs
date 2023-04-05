using Newtonsoft.Json;
using SoundShapesServer.Enums;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses;

public class UserResponse
{
    public string id { get; set; }
    public string type { get; set; }
    public string displayName { get; set; }
}
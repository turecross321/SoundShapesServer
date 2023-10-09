using Newtonsoft.Json;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Authentication;

public class TokenUserResponse : IResponse
{
    public TokenUserResponse(GameUser user)
    {
        Username = user.Username;
        Id = user.Id;
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("display_name")] public string Username { get; set; }
}
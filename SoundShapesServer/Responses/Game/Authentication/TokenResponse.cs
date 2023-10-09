using Newtonsoft.Json;
using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Game.Authentication;

public class TokenResponse : IResponse
{
    public TokenResponse(AuthToken token)
    {
        // Game doesn't actually listen to expiry date, until it disconnects, and it will only try to get a new token
        // if the old one is already expired. This can only lead to problems, hence this always being set to 0
        ExpirationDate = 0; 
        Id = token.Id;
        User = new TokenUserResponse(token.User);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("expires")] public long ExpirationDate { get; set; }
    [JsonProperty("person")] public TokenUserResponse User { get; set; }
}
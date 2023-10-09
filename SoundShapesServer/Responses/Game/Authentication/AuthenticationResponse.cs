using Newtonsoft.Json;
using SoundShapesServer.Types.Authentication;

namespace SoundShapesServer.Responses.Game.Authentication;

public class AuthenticationResponse : IResponse
{
    public AuthenticationResponse(AuthToken token)
    {
        Token = new TokenResponse(token);
    }

    [JsonProperty("session")] public TokenResponse Token { get; set; }
}
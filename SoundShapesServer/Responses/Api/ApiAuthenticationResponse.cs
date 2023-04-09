namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
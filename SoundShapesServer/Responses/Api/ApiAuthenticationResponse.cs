namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public string Id { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
}
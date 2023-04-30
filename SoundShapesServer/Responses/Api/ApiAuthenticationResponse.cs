namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public ApiAuthenticationResponse(string id, DateTimeOffset expiresAtUtc, string userId, string username)
    {
        Id = id;
        ExpiresAtUtc = expiresAtUtc;
        UserId = userId;
        Username = username;
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
    public string UserId { get; }
    public string Username { get;  }
}
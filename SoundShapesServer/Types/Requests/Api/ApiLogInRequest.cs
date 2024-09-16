namespace SoundShapesServer.Types.Requests.Api;

public record ApiLogInRequest : IApiRequest
{
    public required string Email { get; init; }
    public required string PasswordSha512 { get; init; }
}
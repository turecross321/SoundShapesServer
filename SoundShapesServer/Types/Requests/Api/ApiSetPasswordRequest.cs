namespace SoundShapesServer.Types.Requests.Api;

public record ApiSetPasswordRequest : IApiRequest
{
    public required string Code { get; init; }
    public required string PasswordSha512 { get; init; }
}
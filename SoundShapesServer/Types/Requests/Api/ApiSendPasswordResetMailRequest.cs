namespace SoundShapesServer.Types.Requests.Api;

public record ApiSendPasswordResetMailRequest : IApiRequest
{
    public required string Email { get; init; }
}
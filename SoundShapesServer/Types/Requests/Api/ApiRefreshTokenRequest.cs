namespace SoundShapesServer.Types.Requests.Api;

public record ApiRefreshTokenRequest : IApiRequest
{
    public required Guid RefreshTokenId { get; init; }
}
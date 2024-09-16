namespace SoundShapesServer.Types.Requests.Api;

public record ApiCodeRequest : IApiRequest
{
    public required string Code { get; init; }
}
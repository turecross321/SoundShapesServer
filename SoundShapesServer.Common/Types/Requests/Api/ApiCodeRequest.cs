namespace SoundShapesServer.Common.Types.Requests.Api;

public record ApiCodeRequest : IApiRequest
{
    public required string Code { get; init; }
}
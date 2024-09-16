namespace SoundShapesServer.Types.Requests.Api;

public record ApiSetEmailRequest: IApiRequest
{
    public required string Code { get; init; }
    public required string NewEmail { get; init; }
}
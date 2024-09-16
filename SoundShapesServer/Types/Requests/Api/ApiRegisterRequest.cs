using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Requests.Api;

public record ApiRegisterRequest : IApiRequest
{
    public required string Code { get; init; }
    public required string Email { get; init; }
    public required string PasswordSha512 { get; init; }
    public required bool AcceptEula { get; init; }
}
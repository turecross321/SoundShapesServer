using SoundShapesServer.Authentication;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiAuthenticationResponse
{
    public ApiAuthenticationResponse(GameSession session)
    {
        Id = session.Id ?? "";
        ExpiresAtUtc = session.ExpiresAt;
        UserId = session.User?.Id ?? "";
        Username = session.User?.Username ?? "";
        PermissionsType = session.User?.PermissionsType ?? (int)Types.PermissionsType.Default;
    }

    public string Id { get; }
    public DateTimeOffset ExpiresAtUtc { get; }
    public string UserId { get; }
    public string Username { get; }
    public int PermissionsType { get; }
}
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiAuthorizationSettingsResponse : IApiResponse
{
    public required bool RpcnAuthorization { get; set; }
    public required bool PsnAuthorization { get; set; }
    public required bool IpAuthorization { get; set; }
}
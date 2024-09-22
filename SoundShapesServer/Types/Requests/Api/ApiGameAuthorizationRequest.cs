namespace SoundShapesServer.Types.Requests.Api;

public record ApiAuthorizationSettingsRequest : IApiRequest
{
    public required bool RpcnAuthorization { get; set; }
    public required bool PsnAuthorization { get; set; }
    public required bool IpAuthorization { get; set; }
}
namespace SoundShapesServer.Requests.Api;
#pragma warning disable CS8618

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetGameAuthenticationSettingsRequest
{
    public bool AllowPsnAuthentication { get; set; }
    public bool AllowRpcnAuthentication { get; set; }
    public bool AllowIpAuthentication { get; set; }
}
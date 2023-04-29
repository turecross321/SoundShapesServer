namespace SoundShapesServer.Responses.Api.IP_Authorization;

public class ApiAuthorizedIpResponse
{
    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}
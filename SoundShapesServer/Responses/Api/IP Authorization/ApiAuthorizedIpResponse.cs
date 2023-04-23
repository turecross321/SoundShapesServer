namespace SoundShapesServer.Responses.Api;

public class ApiAuthorizedIpResponse
{
    public string IpAddress { get; set; }
    public bool OneTimeUse { get; set; }
}
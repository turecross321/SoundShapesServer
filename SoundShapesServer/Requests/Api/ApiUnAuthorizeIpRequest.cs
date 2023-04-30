namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiUnAuthorizeIpRequest
{
    public ApiUnAuthorizeIpRequest(string ipAddress)
    {
        IpAddress = ipAddress;
    }

    public string IpAddress { get; }
}
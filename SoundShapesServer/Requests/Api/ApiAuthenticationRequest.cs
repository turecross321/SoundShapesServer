namespace SoundShapesServer.Requests.Api;


public class ApiAuthenticationRequest
{
    public string Username { get; set; }
    public string PasswordSha512 { get; set; }
}
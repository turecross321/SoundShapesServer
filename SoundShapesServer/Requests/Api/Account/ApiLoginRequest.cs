namespace SoundShapesServer.Requests.Api.Account;


public class ApiLoginRequest
{
    public string Email { get; set; }
    public string PasswordSha512 { get; set; }
}
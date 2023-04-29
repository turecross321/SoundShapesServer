namespace SoundShapesServer.Requests.Api.Account;

public class ApiSetPasswordRequest
{
    public string NewPasswordSha512 { get; set; }
}
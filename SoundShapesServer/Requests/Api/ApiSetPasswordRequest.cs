namespace SoundShapesServer.Requests.Api;

public class ApiSetPasswordRequest
{
    public string NewPasswordSha512 { get; set; }
}
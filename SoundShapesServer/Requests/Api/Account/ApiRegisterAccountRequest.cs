namespace SoundShapesServer.Requests.Api.Account;

public class ApiRegisterAccountRequest
{
    public string RegistrationCode { get; set; }
    public string Email { get; set; }
    public string PasswordSha512 { get; set; }
    public bool AcceptEula { get; set; }
}
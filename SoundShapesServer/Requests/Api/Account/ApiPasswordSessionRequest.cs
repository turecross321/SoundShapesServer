namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPasswordSessionRequest
{
    public ApiPasswordSessionRequest(string email)
    {
        Email = email;
    }

    public string Email { get; }
}
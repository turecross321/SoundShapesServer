namespace SoundShapesServer.Requests.Api.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetEmailRequest
{
    public ApiSetEmailRequest(string newEmail)
    {
        NewEmail = newEmail;
    }

    public string NewEmail { get; }
}
using System.Net;

namespace SoundShapesServer.Common.Types.Responses.Api.ApiTypes.Errors;

public class ApiInternalServerError : ApiError
{
    public const string CouldNotBcryptPasswordWhen = "Could not BCrypt the given password.";
    public static readonly ApiInternalServerError CouldNotBcryptPassword = new(CouldNotBcryptPasswordWhen);
    
    public const string CouldNotSendEmailWhen = "Could not send email. Please try again.";
    public static readonly ApiInternalServerError CouldNotSendEmail = new(CouldNotSendEmailWhen);

    private ApiInternalServerError(string message) : base(message, HttpStatusCode.InternalServerError)
    {
    }
}
using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiInternalServerError : ApiError
{
    public const string CouldNotSendEmailWhen = "Could not send email. Please try again.";
    public static readonly ApiInternalServerError CouldNotSendEmail = new(CouldNotSendEmailWhen);

    public ApiInternalServerError(string message) : base(message, HttpStatusCode.InternalServerError)
    {
    }
}
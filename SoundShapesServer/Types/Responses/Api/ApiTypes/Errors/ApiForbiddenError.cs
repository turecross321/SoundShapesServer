namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiForbiddenError(string message) : ApiError(message, Forbidden)
{
    public const string EmailOrPasswordIsWrongWhen = "The email address or password was incorrect.";
    public static readonly ApiForbiddenError EmailOrPasswordIsWrong = new(EmailOrPasswordIsWrongWhen);
}
namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiForbiddenError : ApiError
{
    public const string EmailOrPasswordIsWrongWhen = "The email address or password was incorrect.";
    public static readonly ApiForbiddenError EmailOrPasswordIsWrong = new(EmailOrPasswordIsWrongWhen);
    
    private ApiForbiddenError(string message) : base(message, Forbidden)
    {
    }
}
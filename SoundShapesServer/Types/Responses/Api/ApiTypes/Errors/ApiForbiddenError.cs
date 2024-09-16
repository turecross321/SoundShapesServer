using System.Net;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiForbiddenError : ApiError
{
    public const string EmailOrPasswordIsWrongWhen = "The email address or password was incorrect.";
    public static readonly ApiForbiddenError EmailOrPasswordIsWrong = new(EmailOrPasswordIsWrongWhen);
    public const string FollowYourselfWhen = "You cannot follow yourself.";
    public static readonly ApiForbiddenError FollowYourself = new(FollowYourselfWhen);
    
    private ApiForbiddenError(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}
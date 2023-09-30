using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiForbiddenError : ApiError
{
    public const string EmailOrPasswordIsWrongWhen = "The email address or password was incorrect.";
    public static readonly ApiForbiddenError EmailOrPasswordIsWrong = new(EmailOrPasswordIsWrongWhen);
    public const string FollowYourselfWhen = "You cannot follow yourself.";
    public static readonly ApiForbiddenError FollowYourself = new(FollowYourselfWhen);
    
    public ApiForbiddenError(string message) : base(message, HttpStatusCode.Forbidden)
    {
    }
}
using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiMethodNotAllowedError : ApiError
{
    public const string PunishYourselfWhen = "You can not punish yourself.";
    public static readonly ApiMethodNotAllowedError PunishYourself = new(PunishYourselfWhen);

    public ApiMethodNotAllowedError(string message) : base(message, HttpStatusCode.MethodNotAllowed)
    {
    }
}
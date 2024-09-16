using System.Net;

namespace SoundShapesServer.Types.ServerResponses.Api.ApiTypes.Errors;

public class ApiMethodNotAllowedError : ApiError
{
    public const string PunishYourselfWhen = "You can not punish yourself.";
    public static readonly ApiMethodNotAllowedError PunishYourself = new(PunishYourselfWhen);

    private ApiMethodNotAllowedError(string message) : base(message, HttpStatusCode.MethodNotAllowed)
    {
    }
}
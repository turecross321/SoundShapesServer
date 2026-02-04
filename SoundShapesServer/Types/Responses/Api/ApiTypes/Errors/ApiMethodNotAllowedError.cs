namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiMethodNotAllowedError(string message) : ApiError(message, MethodNotAllowed)
{
    public const string PunishYourselfWhen = "You can not punish yourself.";
    public static readonly ApiMethodNotAllowedError PunishYourself = new(PunishYourselfWhen);
}
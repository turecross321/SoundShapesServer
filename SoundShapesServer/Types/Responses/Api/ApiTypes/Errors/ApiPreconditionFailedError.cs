namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiPreconditionFailedError(string message) : ApiError(message, PreconditionFailed)
{
    public const string IpAuthorizationDisabledWhen = "IP Authorization is disabled.";
    public static readonly ApiPreconditionFailedError IpAuthorizationDisabled = new(IpAuthorizationDisabledWhen);
}
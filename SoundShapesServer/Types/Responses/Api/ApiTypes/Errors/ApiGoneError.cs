namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiGoneError(string message) : ApiError(message, Gone)
{
    public const string MissingFileWhen = "File is unexpectedly gone.";
    public static readonly ApiGoneError MissingFile = new(MissingFileWhen);
}
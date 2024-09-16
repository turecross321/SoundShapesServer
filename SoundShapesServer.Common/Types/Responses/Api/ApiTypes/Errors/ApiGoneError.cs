using System.Net;

namespace SoundShapesServer.Common.Types.Responses.Api.ApiTypes.Errors;

public class ApiGoneError : ApiError
{
    public const string MissingFileWhen = "File is unexpectedly gone.";
    public static readonly ApiGoneError MissingFile = new(MissingFileWhen);

    private ApiGoneError(string message) : base(message, HttpStatusCode.Gone)
    {
    }
}
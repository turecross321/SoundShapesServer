using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiGoneError : ApiError
{
    public const string MissingFileWhen = "File is unexpectedly gone.";
    public static readonly ApiGoneError MissingFile = new(MissingFileWhen);

    public ApiGoneError(string message) : base(message, HttpStatusCode.Gone)
    {
    }
}
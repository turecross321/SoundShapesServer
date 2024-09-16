using System.Net;
using AttribDoc;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;

namespace SoundShapesServer.Types.ServerResponses.Api.ApiTypes.Errors;

public class ApiError
{
    public ApiError(string message, HttpStatusCode code = HttpStatusCode.BadRequest)
    {
        Name = GetType().Name;
        Message = message;
        StatusCode = code;
    }
    public string Name { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public static implicit operator Response(ApiError error) 
        => new(error, ContentType.Json, error.StatusCode);
    
    public ApiError(){}
    
    private ApiError FromError(Error error)
    {
        return new ApiError
        {
            Name = error.Name,
            Message = error.OccursWhen
        };
    }
    public List<ApiError> FromErrorList(List<Error> errors)
    {
        return errors.Select(FromError).ToList();
    }
}
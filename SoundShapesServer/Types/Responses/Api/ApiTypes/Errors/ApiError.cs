using System.Net;
using AttribDoc;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiError
{
    public ApiError(string message, HttpStatusCode code = BadRequest)
    {
        this.Name = this.GetType().Name;
        this.Message = message;
        this.StatusCode = code;
    }
    public string Name { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }

    public static implicit operator Response(ApiError error) 
        => new(error, ContentType.Json, error.StatusCode);
    
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // ReSharper disable once MemberCanBePrivate.Global
    public ApiError(){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    private ApiError FromError(Error error)
    {
        return new ApiError
        {
            Name = error.Name,
            Message = error.OccursWhen,
        };
    }
    public List<ApiError> FromErrorList(IEnumerable<Error> errors)
    {
        return errors.Select(this.FromError).ToList();
    }
}
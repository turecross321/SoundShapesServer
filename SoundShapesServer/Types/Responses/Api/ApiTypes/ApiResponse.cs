using System.Net;
using Bunkum.Core.Responses;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiResponse<T> : IHasResponseCode where T : class
{
    [Obsolete("Only used for deserialization")]
    public ApiResponse()
    {
        
    }
    
    protected ApiResponse(T data)
    {
        Success = true;
        Data = data;
        Error = null;

        StatusCode = HttpStatusCode.OK;
    }

    protected ApiResponse(ApiError error)
    {
        Success = false;
        Data = null;
        Error = error;

        StatusCode = error.StatusCode;
    }

    public static implicit operator ApiResponse<T>(T data)
    {
        return new ApiResponse<T>(data);
    }
    
    public static implicit operator ApiResponse<T>(ApiError error)
    {
        return new ApiResponse<T>(error);
    }
    
    public HttpStatusCode StatusCode { get; set; }
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ApiError? Error { get; set; }
}
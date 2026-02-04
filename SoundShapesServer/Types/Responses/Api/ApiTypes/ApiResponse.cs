using System.Net;
using System.Text.Json.Serialization;
using Bunkum.Core.Responses;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiResponse<T> : IHasResponseCode where T : class
{
    [Obsolete("Only used for deserialization")]
    public ApiResponse()
    {
        
    }
    
    public ApiResponse(T data)
    {
        this.Success = true;
        this.Data = data;
        this.Error = null;
        
        this.StatusCode = OK;
    }
    
    public ApiResponse(T data, HttpStatusCode code)
    {
        this.Success = true;
        this.Data = data;
        this.Error = null;
        
        this.StatusCode = code;
    }

    public ApiResponse(ApiError error)
    {
        this.Success = false;
        this.Data = null;
        this.Error = error;
        
        this.StatusCode = error.StatusCode;
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
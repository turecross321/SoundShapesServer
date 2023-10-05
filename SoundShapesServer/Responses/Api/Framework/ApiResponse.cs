using System.Net;
using Bunkum.Core.Responses;
using SoundShapesServer.Responses.Api.Framework.Errors;

namespace SoundShapesServer.Responses.Api.Framework;

public class ApiResponse<T> : IHasResponseCode where T : class
{
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

    public static implicit operator ApiResponse<T>(T? data)
    {
        if (data == null) 
            return new ApiResponse<T>(new ApiError("Data was null, maybe internal validation failed?"));
        return new ApiResponse<T>(data);
    }
    
    public static implicit operator ApiResponse<T>(ApiError error)
    {
        return new ApiResponse<T>(error);
    }
    
    public HttpStatusCode StatusCode { get; }
    public bool Success { get; private init; }
    public T? Data { get; private init; }
    public ApiError? Error { get; private init; }
}
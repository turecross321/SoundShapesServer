using SoundShapesServer.Responses.Api.Framework.Errors;

namespace SoundShapesServer.Responses.Api.Framework;

public class ApiOkResponse : ApiResponse<ApiEmptyResponse>
{
    public ApiOkResponse() : base(new ApiEmptyResponse())
    {}

    public ApiOkResponse(ApiError error) : base(error)
    {}
    
    public static implicit operator ApiOkResponse(ApiError error) => new(error);
}
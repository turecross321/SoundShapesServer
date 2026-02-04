using System.Net;
using BunkumOpenApi.Attributes;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Attributes;

public class DocApiResponseAttribute : DocResponseAttribute
{
    public DocApiResponseAttribute(HttpStatusCode code = HttpStatusCode.OK, string? description = null)
    {
        this.StatusCode = code;
        this.Description = description;
        this.Schema = typeof(ApiResponse<ApiEmptyResponse>);
    }
    
    public DocApiResponseAttribute(Type schema, string? description = null)
    {
        this.StatusCode = HttpStatusCode.OK;
        this.Schema = typeof(ApiResponse<>).MakeGenericType(schema);
        this.Description = description;
        this.Example = this.GetExample(schema, HttpStatusCode.OK);
    }
    
    public DocApiResponseAttribute(HttpStatusCode code, Type schema, string? description = null)
    {
        this.StatusCode = code;
        this.Schema = typeof(ApiResponse<>).MakeGenericType(schema);
        this.Description = description;
        this.Example = this.GetExample(schema, code);
    }
    
    private object GetExample(Type schema, HttpStatusCode code)
    {
        Type exampleType = typeof(IHasExample<>).MakeGenericType(schema);
        
        if (!exampleType.IsAssignableFrom(schema))
            throw new InvalidOperationException($"{schema.Name} must inherit from {exampleType.Name}.");
        object example = schema.GetMethod("Example")!.Invoke(null, null)!;
        
        Type apiResponseType = typeof(ApiResponse<>).MakeGenericType(schema);
        return Activator.CreateInstance(apiResponseType, [example, code])!;
    }
}
using System.Net;
using System.Reflection;
using BunkumOpenApi.Attributes;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Attributes;

public class DocApiListResponseAttribute : DocResponseAttribute
{
    public DocApiListResponseAttribute(Type schema, string? description = null)
    {
        this.StatusCode = HttpStatusCode.OK;
        this.Schema = typeof(ApiListResponse<>).MakeGenericType(schema);
        this.Description = description;
        this.Example = this.GetExample(schema, HttpStatusCode.OK);
    }
    
    public DocApiListResponseAttribute(HttpStatusCode code, Type schema, string? description = null)
    {
        this.StatusCode = code;
        this.Schema = typeof(ApiListResponse<>).MakeGenericType(schema);
        this.Description = description;
        this.Example = this.GetExample(schema, code);
    }
    
    private object GetExample(Type schema, HttpStatusCode code)
    {
        Type exampleType = typeof(IHasExample<>).MakeGenericType(schema);
        
        if (!exampleType.IsAssignableFrom(schema))
            throw new InvalidOperationException($"{schema.Name} must inherit from {exampleType.Name}.");
        object example = schema.GetMethod("Example")!.Invoke(null, null)!;
        
        Type apiResponseType = typeof(ApiListResponse<>).MakeGenericType(schema);
        
        Type listGenericType = typeof(List<>).MakeGenericType(schema);
        object listWithExample = Activator.CreateInstance(listGenericType)!;
        MethodInfo addMethod = listGenericType.GetMethod("Add", [schema])!;
        addMethod.Invoke(listWithExample, [example]);
        
        return Activator.CreateInstance(apiResponseType, [listWithExample, code])!;
    }
}
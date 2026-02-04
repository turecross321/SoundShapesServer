using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DocApiErrorResponseAttribute : Attribute
{
    public DocApiErrorResponseAttribute(Type type, string when)
    {
        if (!typeof(ApiError).IsAssignableFrom(type))
        {
            throw new Exception("Error type is not or does not inherit ApiError");
        }
        
        object[] constructorArgs = [when];
        this.Error = (ApiError)Activator.CreateInstance(type, constructorArgs)!;
    }
    
    public ApiError Error { get; set; }
}
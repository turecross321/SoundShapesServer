using NSwag;

namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocBodyParameterAttribute(Type schema, bool required = true) : DocParameterAttribute(schema, required, OpenApiParameterKind.Body)
{
    
}
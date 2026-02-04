using NSwag;

namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
// todo: document explode
public class DocPathParameterAttribute(string name, Type schema, string description = "", bool required = false, bool explode = false) 
    : DocParameterAttribute(name, schema, OpenApiParameterKind.Path, description, required, explode)
{
}
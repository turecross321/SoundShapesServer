using NSwag;

namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocQueryParameterAttribute(string name, Type schema, string description = "", bool required = false, bool explode = false) 
    : DocParameterAttribute(name, schema, OpenApiParameterKind.Query, description, required, explode)
{
}
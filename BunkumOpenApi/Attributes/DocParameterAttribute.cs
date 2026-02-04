using NJsonSchema;
using NSwag;

namespace BunkumOpenApi.Attributes;


// todo: doc
[AttributeUsage(AttributeTargets.Method)]
public class DocParameterAttribute : Attribute 
{
    public DocParameterAttribute(string name, Type schema, OpenApiParameterKind kind, string description = "", bool required = false, bool explode = false)
    {
        this.Name = name;
        this.Schema = schema;
        this.Kind = kind;
        this.Description = description;
        this.Required = required;
        this.Explode = explode;
    }
    
    public DocParameterAttribute(Type schema, bool required, OpenApiParameterKind kind)
    {
        this.Schema = schema;
        this.Required = required;
        this.Kind = kind;
    }
    
    public string? Name { get; set; }
    public Type Schema { get; set; }
    public OpenApiParameterKind Kind { get; set; }
    public string? Description { get; set; }
    public bool Required { get; set; }
    public bool Explode { get; set; }
    
    public OpenApiParameter ToOpenApiParameter()
    {
        return new OpenApiParameter()
        {
            Name = this.Name, 
            Kind = this.Kind,
            Description = this.Description,
            IsRequired = this.Required,
            Explode = this.Explode,
            Schema = JsonSchema.FromType(this.Schema),
        };
    }
}
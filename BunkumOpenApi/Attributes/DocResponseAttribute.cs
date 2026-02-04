using System.Net;
using System.Reflection;

namespace BunkumOpenApi.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class DocResponseAttribute : Attribute
{
    public DocResponseAttribute(string? description = null, HttpStatusCode code = HttpStatusCode.OK, Type? schema = null, string? exampleMethodName = null)
    {
        schema ??= typeof(object);
        
        this.StatusCode = code;
        this.Schema = schema; 
        this.Description = description;
        
        if (exampleMethodName != null)
        {
            FieldInfo? exampleMember = schema.GetField(exampleMethodName, BindingFlags.Public | BindingFlags.Static);
            if (exampleMember == null) throw new MissingFieldException(schema.Name, exampleMethodName);
            
            this.Example = exampleMember.GetValue(null);
        }
    }
    
    public HttpStatusCode StatusCode { get; set; }
    public Type Schema { get; set; }
    public string? Description { get; set; }
    public object? Example { get; set; }
}
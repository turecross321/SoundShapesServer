using System.Reflection;
using AttribDoc;
using AttribDoc.Attributes;

namespace SoundShapesServer.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocUsesFilterAttribute<T> : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            DocPropertyQueryAttribute? attribute = property.GetCustomAttribute<DocPropertyQueryAttribute>();
            if (attribute != null)
            {
                route.Parameters.Add(new Parameter(attribute.ParameterName, ParameterType.Query, attribute.Summary));
            }
        }
    }
}
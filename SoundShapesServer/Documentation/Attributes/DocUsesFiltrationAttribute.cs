using System.Reflection;
using AttribDoc;
using AttribDoc.Attributes;
using SoundShapesServer.Attributes;

namespace SoundShapesServer.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocUsesFiltrationAttribute<T> : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            FilterPropertyAttribute? attribute = property.GetCustomAttribute<FilterPropertyAttribute>();
            if (attribute != null)
            {
                route.Parameters.Add(new Parameter(attribute.ParameterName, ParameterType.Query, attribute.Summary));
            }
        }
    }
}
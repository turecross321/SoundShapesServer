using System.Reflection;
using AttribDoc;
using AttribDoc.Attributes;

namespace SoundShapesServer.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocUsesPageDataAttribute : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Parameters.Add(new Parameter("from", ParameterType.Query, "The amount of items to skip."));
        route.Parameters.Add(new Parameter("count", ParameterType.Query, "The amount of items to take."));
        route.Parameters.Add(new Parameter("descending", ParameterType.Query, "Specifies if list should be ascending or descending."));
    }
}
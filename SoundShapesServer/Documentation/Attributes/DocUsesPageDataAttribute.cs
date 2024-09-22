using System.Reflection;
using AttribDoc;
using AttribDoc.Attributes;

namespace SoundShapesServer.Documentation.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DocUsesPageDataAttribute : DocAttribute
{
    public override void AddDataToRouteDocumentation(MethodInfo method, Route route)
    {
        route.Parameters.Add(new Parameter("skip", ParameterType.Query, "The amount of items to skip."));
        route.Parameters.Add(new Parameter("take", ParameterType.Query, "The amount of items to take."));
        route.Parameters.Add(new Parameter("minimumCreationDate", ParameterType.Query, "The earliest creation date to include."));
        route.Parameters.Add(new Parameter("maximumCreationDate", ParameterType.Query, "The latest creation date to include."));
        route.Parameters.Add(new Parameter("excludeId", ParameterType.Query, "Exclude item with provided ID from result."));
    }
}
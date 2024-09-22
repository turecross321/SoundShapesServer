using System.Reflection;
using AttribDoc;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Extensions;
using SoundShapesServer.Endpoints;

namespace SoundShapesServer.Documentation;

public class ApiDocumentationGenerator : DocumentationGenerator
{
    protected override IEnumerable<MethodInfo> FindMethodsToDocument(Assembly assembly) 
        => assembly.GetTypes()
            .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(EndpointGroup)))
            .SelectMany(t => t.GetMethods())
            .Where(m => m.HasCustomAttribute<ApiEndpointAttribute>())
            .ToList();

    protected override void DocumentRouteHook(MethodInfo method, Route route)
    {
        ApiEndpointAttribute endpoint = method.GetCustomAttribute<ApiEndpointAttribute>()!;
        route.Summary = "No summary provided.";

        route.Method = endpoint.Method.ToString().ToUpper();
        route.RouteUri = endpoint.Route;
        
        AuthenticationAttribute? authentication = method.GetCustomAttribute<AuthenticationAttribute>();
        route.AuthenticationRequired = authentication == null || authentication.Required;
    }
}
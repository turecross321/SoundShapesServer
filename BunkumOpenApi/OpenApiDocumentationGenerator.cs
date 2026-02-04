using System.Reflection;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using BunkumOpenApi.Attributes;
using NJsonSchema;
using NSwag;

namespace BunkumOpenApi;

public class OpenApiDocumentationGenerator(OpenApiInfo info, IList<OpenApiTag>? tags = null)
{
    protected virtual HttpEndpointAttribute? GetEndpointAttribute(MethodInfo method)
    {
        return method.GetCustomAttribute<HttpEndpointAttribute>();
    }
    
    protected virtual void AddAdditionalInformationToOperation(MethodInfo method, OpenApiOperation operation)
    {
        
    }

    // TODO: FIX ENDPOINT HAVING API V1 IN IT.
    
    protected virtual OpenApiServer[] GetServers()
    {
        return [];
    }
    
    public OpenApiDocument GenerateDocument(Assembly assembly)
    {
        OpenApiDocument document = new OpenApiDocument
        {
            Tags = tags,
            Info = info,
        };

        foreach (OpenApiServer server in GetServers())
        {
            document.Servers.Add(server);
        }

        IEnumerable<Type> endpointGroups = assembly.GetTypes()
            .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EndpointGroup)));
        
        foreach (Type group in endpointGroups)
        {
            DocTagAttribute? groupTagAttribute = group.GetCustomAttribute<DocTagAttribute>();
            
            foreach (MethodInfo method in group.GetMethods())
            {
                HttpEndpointAttribute? endpointAttribute = GetEndpointAttribute(method);
                if (endpointAttribute == null)
                    continue;
                
                List<string> tags = GetTagNames(groupTagAttribute, method);
                
                OpenApiOperation operation = new OpenApiOperation
                {
                    Tags = tags,
                    Summary = method.GetCustomAttribute<DocSummaryAttribute>()?.Summary,
                    Description = method.GetCustomAttribute<DocDescriptionAttribute>()?.Description,
                };
                
                AddParametersToOperation(method, operation);
                AddResponsesToOperation(method, operation);
                AddAdditionalInformationToOperation(method, operation);
                
                // Ensure the route exists and add the operation by HTTP method
                if (!document.Paths.ContainsKey(endpointAttribute.FullRoute))
                    document.Paths.Add(endpointAttribute.FullRoute, new OpenApiPathItem());
                
                document.Paths[endpointAttribute.FullRoute].Add(
                    endpointAttribute.Method.ToString(),
                    operation
                );
            }
        }
        
        return document;
    }
    
    private List<string> GetTagNames(DocTagAttribute? groupTag, MethodInfo method)
    {
        List<string> tags = new List<string>();
        
        // Add class-level tag
        if (groupTag != null)
            tags.Add(groupTag.TagName);
        
        // Add method-level tags
        tags.AddRange(method.GetCustomAttributes<DocTagAttribute>().Select(attr => attr.TagName));
        
        return tags;
    }
    
    private void AddParametersToOperation(MethodInfo method, OpenApiOperation operation)
    {
        IEnumerable<DocParameterAttribute>
            parameters = method.GetCustomAttributes<DocParameterAttribute>(inherit: true);
        
        foreach (DocParameterAttribute parameter in parameters)
        {
            operation.Parameters.Add(parameter.ToOpenApiParameter());
        }
    }
    
    private void AddResponsesToOperation(MethodInfo method, OpenApiOperation operation)
    {
        IEnumerable<DocResponseAttribute> responses = method.GetCustomAttributes<DocResponseAttribute>();
        
        foreach (DocResponseAttribute response in responses)
        {
            object?[] examples = [];
            if (response.Example != null)
                examples = [response.Example];
            
            operation.Responses.Add(((int)response.StatusCode).ToString(), new OpenApiResponse
            {
                Schema = JsonSchema.FromType(response.Schema),
                Description = response.Description,
                Examples = examples,
            });
        }
    }
}
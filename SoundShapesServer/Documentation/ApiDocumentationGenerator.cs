using System.Net;
using System.Reflection;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using BunkumOpenApi;
using BunkumOpenApi.Attributes;
using NJsonSchema;
using NSwag;
using SoundShapesServer.Attributes;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Documentation;

public class ApiDocumentationGenerator() : OpenApiDocumentationGenerator(ApiInfo, ApiTags.Tags)
{
    private static readonly OpenApiInfo ApiInfo = new OpenApiInfo()
    {
        Title = "Sound Shapes Server API",
        Description = "A custom server for Sound Shapes",
        Version = "v1",
    };
    
    
    protected override HttpEndpointAttribute? GetEndpointAttribute(MethodInfo method)
    {
        return method.GetCustomAttribute<ApiEndpointAttribute>();
    }
    
    protected override void AddAdditionalInformationToOperation(MethodInfo method, OpenApiOperation operation)
    {
        AddApiPageDataParameters(method, operation);
        AddApiErrorsToOperation(method, operation);
    }
    
    private void AddApiErrorsToOperation(MethodInfo method, OpenApiOperation operation)
    {
        IEnumerable<DocApiErrorResponseAttribute> errors = method.GetCustomAttributes<DocApiErrorResponseAttribute>();
        IEnumerable<IGrouping<HttpStatusCode, DocApiErrorResponseAttribute>> groupedByStatus =
            errors.GroupBy(e => e.Error.StatusCode);
        
        foreach (IGrouping<HttpStatusCode, DocApiErrorResponseAttribute> group in groupedByStatus)
        {
            IEnumerable<ApiResponse<ApiEmptyResponse>> examples =
                group.Select(e => new ApiResponse<ApiEmptyResponse>(e.Error));
            
            OpenApiResponse response = new OpenApiResponse
            {
                Schema = JsonSchema.FromType(typeof(ApiResponse<ApiEmptyResponse>)),
                Examples = examples,
            };
            
            operation.Responses.Add(group.Key.ToString(), response);
        }
    }
    
    private void AddApiPageDataParameters(MethodInfo method, OpenApiOperation operation)
    {
        DocApiUsesPageDataAttribute? pageData = method.GetCustomAttribute<DocApiUsesPageDataAttribute>();
        if (pageData == null)
            return;
        
        DocParameterAttribute[] parameters = [
            new DocQueryParameterAttribute("skip", typeof(int), "The amount of items to skip."),
            new DocQueryParameterAttribute("take", typeof(int), "The amount of items to take."),
            new DocQueryParameterAttribute("minimumCreationDate", typeof(DateTimeOffset), "The earliest creation date to include."),
            new DocQueryParameterAttribute("maximumCreationDate", typeof(DateTimeOffset), "The latest creation date to include."),
            new DocQueryParameterAttribute("excludeId", typeof(string), "Exclude item with provided ID from result.", explode:true),
        ];
        
        foreach (DocParameterAttribute parameter in parameters)
        {
            operation.Parameters.Add(parameter.ToOpenApiParameter());
        }
    }
}
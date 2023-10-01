using System.Reflection;
using Bunkum.Core.Services;
using NotEnoughLogs;
using SoundShapesServer.Documentation;
using SoundShapesServer.Responses.Api.Framework.Documentation;

namespace SoundShapesServer.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public class DocumentationService : EndpointService
{
    internal DocumentationService(Logger logger) : base(logger)
    {}

    public override void Initialize()
    {
        AttribDoc.Documentation documentation = _generator.Document(Assembly.GetExecutingAssembly());
        _docs.AddRange(ApiRouteResponse.FromRouteList(documentation.Routes.OrderBy(r => r.RouteUri)));
    }

    private readonly DocumentationGenerator _generator = new();
    
    private readonly List<ApiRouteResponse> _docs = new();
    public IEnumerable<ApiRouteResponse> Documentation => _docs.AsReadOnly();
}
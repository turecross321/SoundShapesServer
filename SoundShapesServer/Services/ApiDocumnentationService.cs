using System.Reflection;
using AttribDoc;
using Bunkum.Core.Services;
using NotEnoughLogs;
using SoundShapesServer.Documentation;

namespace SoundShapesServer.Services;

public class ApiDocumentationService : EndpointService
{
    internal ApiDocumentationService(Logger logger) : base(logger)
    {}

    public override void Initialize()
    {
        AttribDoc.Documentation documentation = _generator.Document(Assembly.GetExecutingAssembly());
        _docs.AddRange(documentation.Routes.OrderBy(r => r.RouteUri));
    }

    private readonly ApiDocumentationGenerator _generator = new();
    
    private readonly List<Route> _docs = new();
    public IEnumerable<Route> Routes => _docs.AsReadOnly();
}
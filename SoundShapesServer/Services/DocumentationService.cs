using System.Reflection;
using AttribDoc;
using Bunkum.Core.Services;
using NotEnoughLogs;
using DocumentationGenerator = SoundShapesServer.Documentation.DocumentationGenerator;

namespace SoundShapesServer.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public class DocumentationService : EndpointService
{
    internal DocumentationService(Logger logger) : base(logger)
    {}

    public override void Initialize()
    {
        AttribDoc.Documentation documentation = _generator.Document(Assembly.GetExecutingAssembly());
        _docs.AddRange(documentation.Routes.OrderBy(r => r.RouteUri));
    }

    private readonly DocumentationGenerator _generator = new();
    
    private readonly List<Route> _docs = new();
    public IEnumerable<Route> Documentation => _docs.AsReadOnly();
}
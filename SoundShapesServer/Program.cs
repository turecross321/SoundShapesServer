using System.Reflection;
using Bunkum.Core;
using NSwag;
using SoundShapesServer;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation;

ApiDocumentationGenerator generator = new ApiDocumentationGenerator();
OpenApiDocument doc = generator.GenerateDocument(Assembly.GetExecutingAssembly());
Console.WriteLine(doc.ToJson());

return;
BunkumConsole.AllocateConsole();

using SSServer<GameDatabaseProvider> server = new();

server.Start();
await Task.Delay(-1);
using System.Reflection;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Services;

BunkumHttpServer server = new();

server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());

using RealmDatabaseProvider databaseProvider = new();

server.UseDatabaseProvider(databaseProvider);
server.AddStorageService<FileSystemDataStore>();

server.AddAuthenticationService(new SessionProvider(), true);
server.UseJsonConfig<GameServerConfig>("gameServer.json");

server.AddMiddleware<CrossOriginMiddleware>();
server.AddService<EmailService>();

await server.StartAndBlockAsync();
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

server.AddAuthenticationService(new SessionProvider(), true);
server.AddStorageService<FileSystemDataStore>();
server.AddService<EmailService>();

server.UseJsonConfig<GameServerConfig>("gameServer.json");

server.AddMiddleware<CrossOriginMiddleware>();

await server.StartAndBlockAsync();
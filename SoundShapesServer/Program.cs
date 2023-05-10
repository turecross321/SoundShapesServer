using System.Reflection;
using Bunkum.HttpServer;
using Bunkum.HttpServer.RateLimit;
using Bunkum.HttpServer.Storage;
using SoundShapesServer;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Services;

using GameDatabaseProvider databaseProvider = new();

// Level Importing
FileSystemDataStore dataStore = new ();
databaseProvider.Initialize();
LevelImporting.ImportLevels(databaseProvider.GetContext(), dataStore);

BunkumHttpServer server = new();

server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());

server.UseJsonConfig<GameServerConfig>("gameServer.json");

server.UseDatabaseProvider(databaseProvider);
server.AddStorageService(dataStore);
server.AddAuthenticationService(new SessionProvider(), true);
server.AddRateLimitService(new RateLimitSettings(60, 400, 60)); // Todo: figure out a good balance here between security and usability
server.AddService<EmailService>();

server.AddMiddleware<CrossOriginMiddleware>();
server.AddMiddleware<FileSizeMiddleware>();

await server.StartAndBlockAsync();
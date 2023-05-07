using System.Reflection;
using Bunkum.HttpServer;
using Bunkum.HttpServer.RateLimit;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Services;

BunkumHttpServer server = new();

server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());

using GameDatabaseProvider databaseProvider = new();

server.UseJsonConfig<GameServerConfig>("gameServer.json");

server.UseDatabaseProvider(databaseProvider);

server.AddAuthenticationService(new SessionProvider(), true);
server.AddStorageService<FileSystemDataStore>();
server.AddRateLimitService(new RateLimitSettings(60, 400, 60)); // Todo: figure out a good balance here between security and usability
server.AddService<EmailService>();

server.AddMiddleware<CrossOriginMiddleware>();
server.AddMiddleware<FileSizeMiddleware>();

await server.StartAndBlockAsync();
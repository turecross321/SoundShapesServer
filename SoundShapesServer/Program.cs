using System.Reflection;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Middlewares;

BunkumHttpServer server = new()
{
    AssumeAuthenticationRequired = true,
};

server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());

using RealmDatabaseProvider databaseProvider = new();

server.UseDatabaseProvider(databaseProvider);
server.UseDataStore(new FileSystemDataStore());

server.UseAuthenticationProvider(new SessionProvider());
server.UseJsonConfig<GameServerConfig>("gameServer.json");

server.AddMiddleware<CrossOriginMiddleware>();

await server.StartAndBlockAsync();
using System.Reflection;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;

BunkumHttpServer server = new()
{
    AssumeAuthenticationRequired = true,
};

server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());

using RealmDatabaseProvider databaseProvider = new();

server.UseDatabaseProvider(databaseProvider);
server.UseDataStore(new FileSystemDataStore());

server.UseAuthenticationProvider(new GameAuthenticationProvider());

server.UseJsonConfig<GameServerConfig>("gameServer.json");

await server.StartAndBlockAsync();
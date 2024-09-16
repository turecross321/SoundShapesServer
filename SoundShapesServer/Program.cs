using System.Reflection;
using Bunkum.AutoDiscover.Extensions;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses.Serialization;
using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using NotEnoughLogs;
using NotEnoughLogs.Behaviour;
using SoundShapesServer;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Config;

BunkumServer server = new BunkumHttpServer(new LoggerConfiguration
{
    Behaviour = new QueueLoggingBehaviour(),
#if DEBUG
    MaxLevel = LogLevel.Trace,
#else
    MaxLevel = LogLevel.Info,
#endif
});

server.Initialize = s =>
{
    ServerConfig config = Config.LoadFromJsonFile<ServerConfig>("soundShapesConfig.json", s.Logger);

    IDataStore dataStore = new FileSystemDataStore();
    
    s.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    s.AddStorageService(dataStore);
    s.AddConfig(config);
    
    GameDatabaseProvider databaseProvider = new(config);
    s.UseDatabaseProvider(databaseProvider);

    GameAuthenticationProvider authenticationProvider = new();
    s.AddAuthenticationService(authenticationProvider, true);
    
    s.AddAutoDiscover(config.InstanceSettings.InstanceName, GameEndpointAttribute.RoutePrefix, false, 
        config.InstanceSettings.Description, config.InstanceSettings.BannerUrl);
    
    s.AddService<EmailService>();
    s.AddRateLimitService(new RateLimitSettings(60, 400, 30, "global"));
    
    s.RemoveSerializer<BunkumJsonSerializer>();
    s.AddSerializer<SoundShapesSerializer>();
};

server.Start();
await Task.Delay(-1);
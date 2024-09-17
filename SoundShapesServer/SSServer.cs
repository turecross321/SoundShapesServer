using Bunkum.AutoDiscover.Extensions;
using Bunkum.Core.Authentication;
using Bunkum.Core.Configuration;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses.Serialization;
using Bunkum.Core.Storage;
using Bunkum.EntityFrameworkDatabase;
using Bunkum.Protocols.Http;
using SoundShapesServer.Common;
using SoundShapesServer.Common.Time;
using SoundShapesServer.Common.Types.Config;
using SoundShapesServer.Common.Types.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Services;

namespace SoundShapesServer;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Sound Shapes Server
/// </summary>
public class SSServer<TDatabaseProvider> : ServerBase where TDatabaseProvider : GameDatabaseProvider
{
    protected readonly EntityFrameworkDatabaseProvider<GameDatabaseContext> _databaseProvider;
    protected readonly IDataStore _dataStore;
    protected ServerConfig? _config;
    
    public SSServer(BunkumHttpListener? listener = null,
        Func<TDatabaseProvider>? databaseProvider = null,
        IAuthenticationProvider<DbToken>? authProvider = null,
        IDataStore? dataStore = null,
        ServerConfig? config = null) : base(listener)
    {
        _config ??= Config.LoadFromJsonFile<ServerConfig>("soundShapesConfig.json", this.Logger);
        
        databaseProvider ??= () => (TDatabaseProvider)new GameDatabaseProvider(GetTimeProvider(), _config.PostgresSqlConnectionString);
        dataStore ??= new FileSystemDataStore();

        this._databaseProvider = databaseProvider.Invoke();
        this._databaseProvider.Initialize();
        this._dataStore = dataStore;
        this.Server.AddConfig(_config);

        
        this.SetupInitializer(() =>
        {
            GameDatabaseProvider provider = databaseProvider.Invoke();
            authProvider ??= new GameAuthenticationProvider();

            this.InjectBaseServices(provider, authProvider, this._dataStore);
        });
    }
    
    
    private void InjectBaseServices(GameDatabaseProvider databaseProvider, IAuthenticationProvider<DbToken> authProvider, IDataStore dataStore)
    {
        this.Server.UseDatabaseProvider(databaseProvider);
        this.Server.AddAuthenticationService(authProvider, true);
        this.Server.AddStorageService(dataStore);
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        //this.SetupWorkers();
    }
    
    protected override void SetupServices()
    {
        Server.AddAutoDiscover(_config!.InstanceSettings.InstanceName, GameEndpointAttribute.RoutePrefix, false, 
            _config.InstanceSettings.Description, _config!.InstanceSettings.BannerUrl);
        Server.AddService<EmailService>();
        Server.AddRateLimitService(new RateLimitSettings(60, 400, 30, "global"));
        
        Server.RemoveSerializer<BunkumJsonSerializer>();
        Server.AddSerializer<SoundShapesSerializer>();
    }

    protected override void SetupMiddlewares()
    {
        
    }
    
    protected virtual IDateTimeProvider GetTimeProvider()
    {
        return new SystemDateTimeProvider();
    }

    public override void Dispose()
    {
        this._databaseProvider.Dispose();
        base.Dispose();
    }
    
}
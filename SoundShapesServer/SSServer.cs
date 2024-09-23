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
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Workers;

namespace SoundShapesServer;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Sound Shapes Server
/// </summary>
public class SSServer<TDatabaseProvider> : ServerBase where TDatabaseProvider : GameDatabaseProvider
{
    protected WorkerManager? WorkerManager;
    
    private readonly EntityFrameworkDatabaseProvider<GameDatabaseContext> _databaseProvider;
    private readonly IDataStore _dataStore;
    protected ServerConfig? _config;
    
    public SSServer(BunkumHttpListener? listener = null,
        Func<TDatabaseProvider>? databaseProvider = null,
        IAuthenticationProvider<DbToken>? authProvider = null,
        IDataStore? dataStore = null,
        ServerConfig? config = null) : base(listener)
    {
        config ??= Config.LoadFromJsonFile<ServerConfig>("soundShapesConfig.json", this.Logger);
        this._config = config;
        
        databaseProvider ??= () => (TDatabaseProvider)new GameDatabaseProvider(GetTimeProvider(), _config.PostgresSqlConnectionString);
        dataStore ??= new FileSystemDataStore();

        this._databaseProvider = databaseProvider.Invoke();
        this._databaseProvider.Initialize();
        this._dataStore = dataStore;
        this.Server.AddConfig(_config);

        
        this.SetupInitializer(() =>
        {
            GameDatabaseProvider provider = databaseProvider.Invoke();
            
            this.WorkerManager?.Stop();
            
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
    
    
    // ReSharper disable once RedundantOverriddenMember
    protected override void Initialize()
    {
        base.Initialize();
        SetupWorkers();
    }

    protected override void SetupServices()
    {
        Server.AddAutoDiscover(_config!.InstanceSettings.InstanceName, GameEndpointAttribute.RoutePrefix, false, 
            _config.InstanceSettings.Description, _config!.InstanceSettings.BannerUrl);
        Server.AddService<EmailService>();
        Server.AddRateLimitService(new RateLimitSettings(60, 400, 30, "global"));
        Server.AddService<ApiDocumentationService>();

        Server.RemoveSerializer<BunkumJsonSerializer>();
        Server.AddSerializer<SoundShapesSerializer>();
    }

    protected override void SetupMiddlewares()
    {
        Server.AddMiddleware<CrossOriginMiddleware>();
    }
    
    protected virtual void SetupWorkers()
    {
        WorkerManager = new WorkerManager(this.Logger, this._dataStore, this._databaseProvider);
        this.WorkerManager.AddWorker<ExpiredObjectWorker>();
    }
    
    protected virtual IDateTimeProvider GetTimeProvider()
    {
        return new SystemDateTimeProvider();
    }
    
    /// <inheritdoc/>
    public override void Start()
    {
        this.Server.Start();
        this.WorkerManager?.Start();
    }
    
    /// <inheritdoc/>
    public override void Stop()
    {
        this.Server.Stop();
        this.WorkerManager?.Stop();
    }

    public override void Dispose()
    {
        this._databaseProvider.Dispose();
        base.Dispose();
    }
    
}
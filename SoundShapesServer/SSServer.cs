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
        
        databaseProvider ??= () => (TDatabaseProvider)new GameDatabaseProvider(this.GetTimeProvider(), this._config.PostgresSqlConnectionString);
        dataStore ??= new FileSystemDataStore();

        this._databaseProvider = databaseProvider.Invoke();
        this._databaseProvider.Initialize();
        this._dataStore = dataStore;
        this.Server.AddConfig(this._config);

        
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
        this.SetupWorkers();
    }

    protected override void SetupServices()
    {
        this.Server.AddAutoDiscover(this._config!.InstanceSettings.InstanceName, GameEndpointAttribute.RoutePrefix, false, this._config.InstanceSettings.Description, this._config!.InstanceSettings.BannerUrl);
        this.Server.AddService<EmailService>();
        this.Server.AddRateLimitService(new RateLimitSettings(60, 400, 30, "global"));
        this.Server.AddService<ApiDocumentationService>();
        
        this.Server.RemoveSerializer<BunkumJsonSerializer>();
        this.Server.AddSerializer<SoundShapesSerializer>();
    }

    protected override void SetupMiddlewares()
    {
        this.Server.AddMiddleware<CrossOriginMiddleware>();
    }
    
    protected virtual void SetupWorkers()
    {
        this.WorkerManager = new WorkerManager(this.Logger, this._dataStore, this._databaseProvider);
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
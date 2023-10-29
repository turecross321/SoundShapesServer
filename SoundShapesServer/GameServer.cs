using System.Globalization;
using System.Reflection;
using Bunkum.AutoDiscover.Extensions;
using Bunkum.Core.Authentication;
using Bunkum.Core.Responses;
using Bunkum.Core.Responses.Serialization;
using Bunkum.Core.Storage;
using Bunkum.Listener;
using Bunkum.ProfanityFilter;
using Bunkum.Protocols.Http;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Helpers;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses;
using SoundShapesServer.Services;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer;

public class GameServer
{
    protected readonly BunkumHttpServer ServerInstance;
    protected readonly GameDatabaseProvider DatabaseProvider;
    private readonly IDataStore _dataStore;
    private readonly AuthenticationProvider _authProvider;
    protected GameServerConfig? Config;

    public GameServer(BunkumListener? listener = null,
        GameDatabaseProvider? databaseProvider = null,
        IAuthenticationProvider<GameToken>? authProvider = null,
        IDataStore? dataStore = null)
    {
        databaseProvider ??= new GameDatabaseProvider();
        authProvider ??= new AuthenticationProvider();
        dataStore ??= new FileSystemDataStore();

        DatabaseProvider = databaseProvider;
        _dataStore = dataStore;
        _authProvider = (AuthenticationProvider)authProvider;
        ServerInstance = listener == null ? new BunkumHttpServer() : new BunkumHttpServer(listener);
        
        ServerInstance.UseDatabaseProvider(databaseProvider);
        ServerInstance.AddStorageService(dataStore);
        ServerInstance.AddAuthenticationService(_authProvider, true);
        ServerInstance.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
        
        ServerInstance.RemoveSerializer<BunkumJsonSerializer>();
        ServerInstance.AddSerializer<CustomJsonSerializer>();
    }
    
    public virtual void Start()
    {
        ServerInstance.Start();
    }

    public void Stop()
    {
        ServerInstance.Stop();
    }

    public void Initialize()
    {
        DatabaseProvider.Initialize();
        
        SetUpConfiguration();
        SetUpServices();
        SetUpMiddlewares();
        ServerInstance.AddAutoDiscover(serverBrand: "SoundShapesServer", GameEndpointAttribute.BaseRoute[..^1] );
    }

    protected virtual void SetUpConfiguration()
    {
        Config ??= Bunkum.Core.Configuration.Config.LoadFromJsonFile<GameServerConfig>("gameServer.json", ServerInstance.Logger);
        ServerInstance.AddConfig(Config);
    }

    protected virtual void SetUpServices()
    {
        ServerInstance.AddRateLimitService(new (30, 400, 0, "global"));
        ServerInstance.AddService<DocumentationService>();
        ServerInstance.AddService<EmailService>();
        ServerInstance.AddProfanityService();
        ServerInstance.AddService<MinimumPermissionsService>(_authProvider);
    }

    protected virtual void SetUpMiddlewares()
    {
        ServerInstance.AddMiddleware<CrossOriginMiddleware>();
        ServerInstance.AddMiddleware<FileSizeMiddleware>();
        ServerInstance.AddMiddleware<WebsiteMiddleware>();
    }

    public void SetUpAdminUser()
    {
        GameDatabaseContext database = DatabaseProvider.GetContext();
        
        GameUser adminUser = database.GetAdminUser();
        if (string.IsNullOrEmpty(adminUser.Email))
        {
            Console.WriteLine("Admin user does not have an assigned email address.");
            Console.WriteLine("Enter an email address for the Admin user.");
            
            string? input = Console.ReadLine();
            if (input != null)
            {
                database.SetUserEmail(adminUser, input);
                Console.WriteLine($"Admin user's email has been set to {input}");
            }
        }

        if (!string.IsNullOrEmpty(adminUser.PasswordBcrypt)) return;
        {
            Console.WriteLine("Admin user does not have an assigned password.");
            Console.WriteLine("Enter a password for the Admin user.");
            
            string? input = Console.ReadLine();
            if (input == null) return;
            
            string hashedPassword = ResourceHelper.HashString(input);
            database.SetUserPassword(adminUser, hashedPassword);
            
            Console.WriteLine($"Admin user's password has been set to {input}");
        }
    }

    public void ImportLevels()
    {
        LevelImporting.ImportLevels(DatabaseProvider.GetContext(), _dataStore);
    }

    public void AddOfflineLevels()
    {
        GameDatabaseContext context = DatabaseProvider.GetContext();
        GameUser adminUser = context.GetAdminUser();
        
        foreach (string id in LevelHelper.OfflineLevelIds)
        {
            if (context.GetLevelWithId(id) != null)
                continue;
            PublishLevelRequest request = new (id, 0, new DateTimeOffset(), LevelVisibility.Unlisted);
            context.CreateLevel(adminUser, request, PlatformType.Unknown, false, id);
        }
    }
}
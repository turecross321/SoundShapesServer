using System.Reflection;
using Bunkum.AutoDiscover.Extensions;
using Bunkum.Core.Authentication;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses.Serialization;
using Bunkum.Core.Storage;
using Bunkum.Listener;
using Bunkum.Protocols.Http;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints;
using SoundShapesServer.Helpers;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Serializers;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
using ProfanityService = SoundShapesServer.Services.ProfanityService;

namespace SoundShapesServer;

public class GameServer
{
    private readonly AuthenticationProvider _authProvider;
    private readonly IDataStore _dataStore;
    protected readonly GameDatabaseProvider DatabaseProvider;
    protected readonly BunkumHttpServer ServerInstance;
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
        
        
        ServerInstance = new BunkumHttpServer();
        if (listener != null) 
            ServerInstance.UseListener(listener);

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
        ServerInstance.AddAutoDiscover("SoundShapesServer", GameEndpointAttribute.BaseRoute[..^1]);
    }

    private void SetUpConfiguration()
    {
        Config ??= Bunkum.Core.Configuration.Config.LoadFromJsonFile<GameServerConfig>("gameServer.json",
            ServerInstance.Logger);
        ServerInstance.AddConfig(Config);
    }

    private void SetUpServices()
    {
        ServerInstance.AddRateLimitService(new RateLimitSettings(30, 400, 0, "global"));
        ServerInstance.AddService<DocumentationService>();
        ServerInstance.AddService<EmailService>();
        ServerInstance.AddService<ProfanityService>();
        ServerInstance.AddService<MinimumPermissionsService>(_authProvider);
    }

    private void SetUpMiddlewares()
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

    public void ImportCommunityLevels(string path, bool? overwrite)
    {
        GameDatabaseContext context = DatabaseProvider.GetContext();
        LevelImporter.ImportCommunityLevels(context, _dataStore, path, overwrite ?? false);
    }

    public void ImportCampaignLevels(string path, bool? overwrite)
    {
        GameDatabaseContext context = DatabaseProvider.GetContext();
        LevelImporter.ImportCampaignLevels(context, _dataStore, path, overwrite ?? true);
    }
}
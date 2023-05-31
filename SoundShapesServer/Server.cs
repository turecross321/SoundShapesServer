using System.Reflection;
using Bunkum.CustomHttpListener;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.RateLimit;
using Bunkum.HttpServer.Storage;
using Bunkum.ProfanityFilter;
using SoundShapesServer.Authentication;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Middlewares;
using SoundShapesServer.Services;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer;

public class Server
{
    protected readonly BunkumHttpServer _server;
    protected readonly GameDatabaseProvider _databaseProvider;
    protected readonly IDataStore _dataStore;

    public Server(BunkumHttpListener? listener = null,
        GameDatabaseProvider? databaseProvider = null,
        IAuthenticationProvider<GameUser, GameSession>? authProvider = null,
        IDataStore? dataStore = null)
    {
        databaseProvider ??= new GameDatabaseProvider();
        authProvider ??= new SessionProvider();
        dataStore ??= new FileSystemDataStore();

        _databaseProvider = databaseProvider;
        _dataStore = dataStore;

        _server = listener == null ? new BunkumHttpServer() : new BunkumHttpServer(listener);
        
        _server.UseDatabaseProvider(databaseProvider);
        _server.AddStorageService(dataStore);
        _server.AddAuthenticationService(new SessionProvider(), true);
        
        _server.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public Task StartAndBlockAsync()
    {
        return _server.StartAndBlockAsync();
    }
    
    public void Start()
    {
        _server.Start();
    }

    public void Initialize()
    {
        _databaseProvider.Initialize();
        
        SetUpConfiguration();
        SetUpServices();
        SetUpMiddlewares();

        //SetUpAdminUser();
        //ImportLevels();
    }

    protected virtual void SetUpConfiguration()
    {
        _server.UseJsonConfig<GameServerConfig>("gameServer.json");
    }

    protected virtual void SetUpServices()
    {
        _server.AddRateLimitService(new RateLimitSettings(30, 400, 0)); // Todo: figure out a good balance here between security and usability
        _server.AddService<EmailService>();
        _server.AddProfanityService();
    }

    protected virtual void SetUpMiddlewares()
    {
        _server.AddMiddleware<CrossOriginMiddleware>();
        _server.AddMiddleware<FileSizeMiddleware>();
        _server.AddMiddleware<WebsiteMiddleware>();
    }

    private void SetUpAdminUser()
    {
        GameDatabaseContext database = _databaseProvider.GetContext();
        
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

    private void ImportLevels()
    {
        LevelImporting.ImportLevels(_databaseProvider.GetContext(), _dataStore);
    }
}
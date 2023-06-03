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
    protected readonly BunkumHttpServer ServerInstance;
    protected readonly GameDatabaseProvider DatabaseProvider;
    protected readonly IDataStore DataStore;

    public Server(BunkumHttpListener? listener = null,
        GameDatabaseProvider? databaseProvider = null,
        IAuthenticationProvider<GameUser, GameSession>? authProvider = null,
        IDataStore? dataStore = null)
    {
        databaseProvider ??= new GameDatabaseProvider();
        // ReSharper disable once RedundantAssignment
        authProvider ??= new SessionProvider();
        dataStore ??= new FileSystemDataStore();

        DatabaseProvider = databaseProvider;
        DataStore = dataStore;

        ServerInstance = listener == null ? new BunkumHttpServer() : new BunkumHttpServer(listener);
        
        ServerInstance.UseDatabaseProvider(databaseProvider);
        ServerInstance.AddStorageService(dataStore);
        ServerInstance.AddAuthenticationService(new SessionProvider(), true);
        
        ServerInstance.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public Task StartAndBlockAsync()
    {
        return ServerInstance.StartAndBlockAsync();
    }
    
    public void Start()
    {
        ServerInstance.Start();
    }

    public void Initialize()
    {
        DatabaseProvider.Initialize();
        
        SetUpConfiguration();
        SetUpServices();
        SetUpMiddlewares();
    }

    protected virtual void SetUpConfiguration()
    {
        ServerInstance.UseJsonConfig<GameServerConfig>("gameServer.json");
    }

    protected virtual void SetUpServices()
    {
        ServerInstance.AddRateLimitService(new RateLimitSettings(30, 400, 0)); // Todo: figure out a good balance here between security and usability
        ServerInstance.AddService<EmailService>();
        ServerInstance.AddProfanityService();
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
        LevelImporting.ImportLevels(DatabaseProvider.GetContext(), DataStore);
    }
}
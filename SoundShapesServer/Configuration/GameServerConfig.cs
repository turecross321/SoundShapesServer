using Bunkum.HttpServer.Configuration;
using Bunkum.HttpServer.RateLimit;

namespace SoundShapesServer.Configuration;

public class GameServerConfig : Config
{
    public override int CurrentConfigVersion => 4;
    public override int Version { get; set; }
    
    protected override void Migrate(int oldVer, dynamic oldConfig)
    {
        if (CurrentConfigVersion < 2)
        {
            RequireAuthentication = (bool)oldConfig.ApiAuthentication;
        }

        if (CurrentConfigVersion < 4)
        {
            RateLimitSettings.Bucket = RateLimitSettings.DefaultBucket;
        }
    }
    
    public string EulaText { get; set; } = "Welcome back to Sound Shapes!";
    public string WebsiteUrl { get; set; } = "https://example.com";
    public int LevelPublishLimit = 200;
    public bool RequireAuthentication { get; set; }
    public bool AccountCreation { get; set; } = true;
    public string EmailAddress { get; set; } = "";
    public string EmailPassword { get; set; } = "";
    public string EmailHost { get; set; } = "smtp.gmail.com";
    public int EmailHostPort { get; set; } = 587;
    public bool EmailSsl { get; set; } = true;
    public RateLimitSettings RateLimitSettings = new (30, 400, 0, RateLimitSettings.DefaultBucket);
}
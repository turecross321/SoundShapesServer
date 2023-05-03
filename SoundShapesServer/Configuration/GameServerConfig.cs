using Bunkum.HttpServer.Configuration;

namespace SoundShapesServer.Configuration;

public class GameServerConfig : Config
{
    public override int CurrentConfigVersion => 1;
    public override int Version { get; set; }
    
    protected override void Migrate(int oldVer, dynamic oldConfig)
    {
        
    }
    
    public string EulaText { get; set; } = "Welcome back to Sound Shapes!";
    public string WebsiteUrl { get; set; } = "https://example.com";
    public int LevelPublishLimit = 200;
    public bool ApiAuthentication { get; set; }
    public string EmailAddress { get; set; } = "";
    public string EmailPassword { get; set; } = "";
    public string EmailHost { get; set; } = "smtp.gmail.com";
    public int EmailHostPort { get; set; } = 587;
    public bool EmailSsl { get; set; } = true;
}
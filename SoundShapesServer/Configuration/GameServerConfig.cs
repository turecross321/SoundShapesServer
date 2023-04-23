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
    public bool ApiAuthentication { get; set; }
    public string EmailAddress { get; set; }= "";
    public string Password { get; set; } = "";
    public string EmailHost { get; set; } = "";
    public int EmailHostPort { get; set; }
    public bool EmailSsl { get; set; } = true;
}
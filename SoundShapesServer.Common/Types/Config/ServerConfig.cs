namespace SoundShapesServer.Common.Types.Config;

public class ServerConfig: Bunkum.Core.Configuration.Config
{
    public override int CurrentConfigVersion => 2;
    public override int Version { get; set; }
    
    public string PostgresSqlConnectionString { get; set; } = "Host=localhost;Username=username;Password=password;Database=soundshapes";
    public bool RequireAuthentication { get; set; } = false;
    public string EulaText { get; set; } = "";
    public string RepositoryUrl { get; set; } = "https://github.com/turecross321/SoundShapesServer.git";
    public string WebsiteUrl { get; set; } = "https://sound.ture.fish";
    public int LevelPublishLimit = 200;
    public int MaxUgcBytesPerUser { get; set; }= 128_000_000;

    public InstanceSettings InstanceSettings { get; set; } = new();
    public EmailSettings EmailSettings { get; set; } = new();
    
    protected override void Migrate(int oldVer, dynamic oldConfig)
    {
    }
}
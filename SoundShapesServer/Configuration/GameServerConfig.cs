using Bunkum.HttpServer.Configuration;

namespace SoundShapesServer.Configuration;

public class GameServerConfig : Config
{
    public override int CurrentConfigVersion => 1;
    public override int Version { get; set; }
    
    protected override void Migrate(int oldVer, dynamic oldConfig)
    {
        // throw new NotImplementedException();
    }
    
    public string EulaText { get; set; } = "YO MAN";
}
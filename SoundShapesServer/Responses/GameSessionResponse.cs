namespace SoundShapesServer.Types;

public class GameSessionResponse
{
    public long expires { get; set; }
    public string id { get; set; }
    public SessionUserResponse person { get; set; }
    public Service? service { get; set; }
}
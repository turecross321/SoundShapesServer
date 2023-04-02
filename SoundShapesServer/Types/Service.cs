using Realms;

namespace SoundShapesServer.Types;

public class Service : RealmObject
{
    public string id { get; set; } = Guid.NewGuid().ToString(); 
    public string display_name { get; set; }
}
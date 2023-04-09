using Realms;

namespace SoundShapesServer.Types;

public class Service : RealmObject
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string DisplayName { get; set; }
}
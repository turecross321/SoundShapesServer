using MongoDB.Bson;

namespace SoundShapesServer.Types;

public class Person
{
    public string id { get; set; }
    public string display_name { get; set; }
}
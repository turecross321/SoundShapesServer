using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Serialization;
using MongoDB.Bson;
using Realms;
using SoundShapesServer.Enums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public ItemType type = ItemType.identity;
    public string display_name { get; set; } = string.Empty;
    public string friends { get; set; } = string.Empty;
}
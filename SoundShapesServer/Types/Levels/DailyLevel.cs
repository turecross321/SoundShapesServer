using Realms;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    [PrimaryKey] [Required] public string Id { get; init; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
    public GameUser Author { get; set; }
}
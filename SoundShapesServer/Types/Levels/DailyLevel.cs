using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
        [PrimaryKey]
    [Required] public string Id { get; init; } = "";
    public GameLevel Level { get; init; } = new();
    public DateTimeOffset Date { get; init; }
}
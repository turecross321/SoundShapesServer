using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public DailyLevel(string id, GameLevel level, DateTimeOffset date)
    {
        Id = id;
        Level = level;
        Date = date;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public DailyLevel() {}
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; init; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}
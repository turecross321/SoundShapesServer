using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public DailyLevel(string id, GameLevel level, DateTimeOffset date, GameUser author)
    {
        Id = id;
        Level = level;
        Date = date;
        Author = author;
    }
    
    // Realm cries if this doesn't exist
#pragma warning disable CS8618
    public DailyLevel() {}
#pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; init; }
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
    public GameUser Author { get; set; }
}
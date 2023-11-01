using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public interface ILevelRelation
{
    public DateTimeOffset Date { get; init; }
    public GameUser User { get; set; }
    public GameLevel Level { get; init; }
}
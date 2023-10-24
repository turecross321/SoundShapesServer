using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    public GameLevel OnLevel;
    public GameUser? ByUser;
    public bool? Completed;
    public bool? Obsolete;
}
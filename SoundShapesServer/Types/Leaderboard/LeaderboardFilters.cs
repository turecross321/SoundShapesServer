using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    public LeaderboardFilters(GameLevel onLevel, GameUser? byUser = null, bool? completed = null, bool? obsolete = false)
    {
        OnLevel = onLevel;
        ByUser = byUser;
        Completed = completed;
        Obsolete = obsolete;
    }

    public LeaderboardFilters(GameLevel onLevel)
    {
        OnLevel = onLevel;
        ByUser = null;
        Completed = true;
        Obsolete = false;
    }
    
    public readonly GameLevel OnLevel;
    public readonly GameUser? ByUser;
    public bool? Completed;
    public bool? Obsolete;
}
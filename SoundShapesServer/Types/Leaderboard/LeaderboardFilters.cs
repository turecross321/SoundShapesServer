using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    public LeaderboardFilters(GameLevel onLevel, GameUser? byUser = null, bool? completed = null, bool onlyBest = true)
    {
        OnLevel = onLevel;
        ByUser = byUser;
        Completed = completed;
        OnlyBest = onlyBest;
    }

    public LeaderboardFilters(GameLevel onLevel)
    {
        OnLevel = onLevel;
        ByUser = null;
        Completed = true;
        OnlyBest = true;
    }
    
    public readonly GameLevel OnLevel;
    public readonly GameUser? ByUser;
    public bool? Completed;
    public readonly bool OnlyBest;
}
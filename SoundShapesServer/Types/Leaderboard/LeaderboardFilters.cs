using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    public LeaderboardFilters(string? onLevel = null, GameUser? byUser = null, bool? completed = null, bool onlyBest = true)
    {
        OnLevel = onLevel;
        ByUser = byUser;
        Completed = completed;
        OnlyBest = onlyBest;
    }

    public LeaderboardFilters(string onLevel)
    {
        OnLevel = onLevel;
        ByUser = null;
        Completed = true;
        OnlyBest = true;
    }
    
    public readonly string? OnLevel;
    public readonly GameUser? ByUser;
    public bool? Completed;
    public readonly bool OnlyBest;
}
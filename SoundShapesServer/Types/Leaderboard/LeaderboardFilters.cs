using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Leaderboard;

public class LeaderboardFilters
{
    public LeaderboardFilters(string? onLevel, GameUser? byUser, bool? completed, bool onlyBest)
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
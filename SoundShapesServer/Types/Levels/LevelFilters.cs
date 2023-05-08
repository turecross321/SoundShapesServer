using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    public LevelFilters(GameUser? byUser = null, GameUser? likedByUser = null, GameAlbum? inAlbum = null, DateTimeOffset? inDailyDate = null, string? search = null, GameUser? completedBy = null)
    {
        ByUser = byUser;
        LikedByUser = likedByUser;
        CompletedBy = completedBy;
        InAlbum = inAlbum;
        InDailyDate = inDailyDate;
        Search = search;
    }
    
    public readonly GameUser? ByUser;
    public readonly GameUser? LikedByUser;
    public readonly GameUser? CompletedBy;
    public readonly GameAlbum? InAlbum;
    public DateTimeOffset? InDailyDate;
    public readonly string? Search;
}
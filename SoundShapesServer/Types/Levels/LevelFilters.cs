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
    
    public GameUser? ByUser;
    public GameUser? LikedByUser;
    public GameUser? CompletedBy;
    public GameAlbum? InAlbum;
    public DateTimeOffset? InDailyDate;
    public string? Search; // todo: readonly
}
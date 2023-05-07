using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    public LevelFilters(GameUser? byUser = null, GameUser? likedByUser = null, GameAlbum? inAlbum = null, DateTimeOffset? inDaily = null, string? search = null, bool? completed = null)
    {
        ByUser = byUser;
        LikedByUser = likedByUser;
        InAlbum = inAlbum;
        InDaily = inDaily;
        Search = search;
        Completed = completed;
    }
    
    public GameUser? ByUser;
    public GameUser? LikedByUser;
    public GameAlbum? InAlbum;
    public DateTimeOffset? InDaily;
    public string? Search;
    public bool? Completed;
}
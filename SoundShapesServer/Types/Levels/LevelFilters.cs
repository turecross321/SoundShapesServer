using SoundShapesServer.Endpoints.Api.Levels;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    public LevelFilters(GameUser? byUser = null, GameUser? likedByUser = null, GameAlbum? inAlbum = null, 
        bool? inDaily = null, DateTimeOffset? inDailyDate = null, bool? inLatestDaily = null, string? search = null, 
        GameUser? completedBy = null, int? bpm = null, LevelMusicScale? scale = null, int? transposeValue = null)
    {
        ByUser = byUser;
        LikedByUser = likedByUser;
        CompletedBy = completedBy;
        InAlbum = inAlbum;
        InDaily = inDaily;
        InDailyDate = inDailyDate;
        InLatestDaily = inLatestDaily;
        Search = search;
        Bpm = bpm;
        Scale = scale;
        TransposeValue = transposeValue;
    }
    
    public readonly GameUser? ByUser;
    public readonly GameUser? LikedByUser;
    public readonly GameUser? CompletedBy;
    public readonly GameAlbum? InAlbum;
    public bool? InDaily;
    public DateTimeOffset? InDailyDate;
    public bool? InLatestDaily;
    public readonly string? Search;
    public int? Bpm;
    public LevelMusicScale? Scale;
    public int? TransposeValue;
}
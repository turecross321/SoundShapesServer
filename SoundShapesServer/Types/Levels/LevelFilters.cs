using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Levels;

public class LevelFilters
{
    public LevelFilters(GameUser? byUser = null, GameUser? likedByUser = null, GameAlbum? inAlbum = null, 
        bool? inDaily = null, DateTimeOffset? inDailyDate = null, bool? inLatestDaily = null, string? search = null, 
        GameUser? completedBy = null, int? bpm = null, int? scaleIndex = null, int? transposeValue = null,
        bool? hasCar = null, bool? hasExplodingCar = null)
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
        ScaleIndex = scaleIndex;
        TransposeValue = transposeValue;
        HasCar = hasCar;
        HasExplodingCar = hasExplodingCar;
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
    public int? ScaleIndex;
    public int? TransposeValue;
    public bool? HasCar;
    public bool? HasExplodingCar;
}
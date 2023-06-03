using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelAnalysis
{
    public ApiLevelAnalysis(GameLevel level)
    {
        FileSize = level.FileSize;
        Bpm = level.Bpm;
        TransposeValue = level.TransposeValue;
        ScaleIndex = level.ScaleIndex;
        TotalScreens = level.TotalScreens;
        TotalEntities = level.TotalEntities;
        HasCar = level.HasCar;
        HasExplodingCar = level.HasExplodingCar;    
    }
    
    public long FileSize { get; set; }
    public int Bpm { get; set; }
    public int TransposeValue { get; set; }
    public int ScaleIndex { get; set; }
    public int TotalScreens { get; set; }
    public int TotalEntities { get; set; }
    public bool HasCar { get; set; }
    public bool HasExplodingCar { get; set; }
}
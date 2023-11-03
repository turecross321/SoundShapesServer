using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Responses.Levels;

public class ApiLevelAnalysisResponse : IApiResponse, IDataConvertableFrom<ApiLevelAnalysisResponse, GameLevel>
{
    public required long FileSize { get; set; }
    public required int Bpm { get; set; }
    public required int TransposeValue { get; set; }
    public required ScaleType Scale { get; set; }
    public required int TotalScreens { get; set; }
    public required int TotalEntities { get; set; }
    public required bool HasCar { get; set; }
    public required bool HasExplodingCar { get; set; }
    public required bool HasUfo { get; set; }
    public required bool HasFirefly { get; set; }

    public static ApiLevelAnalysisResponse FromOld(GameLevel old)
    {
        return new ApiLevelAnalysisResponse
        {
            FileSize = old.FileSize,
            Bpm = old.Bpm,
            TransposeValue = old.TransposeValue,
            Scale = old.Scale,
            TotalScreens = old.TotalScreens,
            TotalEntities = old.TotalEntities,
            HasCar = old.HasCar,
            HasExplodingCar = old.HasExplodingCar,
            HasUfo = old.HasUfo,
            HasFirefly = old.HasFirefly
        };
    }

    public static IEnumerable<ApiLevelAnalysisResponse> FromOldList(IEnumerable<GameLevel> oldList)
    {
        return oldList.Select(FromOld);
    }
}
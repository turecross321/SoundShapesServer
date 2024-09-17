namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public interface IApiDbResponse<in TDbEntity, out TSelf> : IApiResponse where TSelf : IApiDbResponse<TDbEntity, TSelf>
{
    public static abstract TSelf FromDb(TDbEntity value);
}
namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public interface IApiDbResponse<in TDbEntity, out TSelf> : IApiResponse
{
    public static abstract TSelf FromDb(TDbEntity item);
}
namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public interface IApiDbResponse<in TDbEntity, out TSelf> : IApiResponse<TSelf> where TSelf : IApiResponse<TSelf>
{
    public static abstract TSelf FromDb(TDbEntity item);
}
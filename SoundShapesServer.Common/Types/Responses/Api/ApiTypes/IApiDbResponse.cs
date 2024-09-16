namespace SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

public interface IApiDbResponse<in TDbEntity, out TSelf> : IApiResponse where TSelf : IApiDbResponse<TDbEntity, TSelf>
{
    public static abstract TSelf FromDb(TDbEntity value);

    public static abstract IEnumerable<TSelf> FromDbList(IEnumerable<TDbEntity> value);
}
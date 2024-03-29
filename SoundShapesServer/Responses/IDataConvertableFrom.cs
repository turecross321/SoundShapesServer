namespace SoundShapesServer.Responses;

public interface IDataConvertableFrom<out TNew, in TOld> where TNew : IDataConvertableFrom<TNew, TOld>
{
    public static abstract TNew FromOld(TOld old);

    public static abstract IEnumerable<TNew> FromOldList(IEnumerable<TOld> oldList);
}
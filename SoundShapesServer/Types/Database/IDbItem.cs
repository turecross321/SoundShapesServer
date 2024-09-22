namespace SoundShapesServer.Types.Database;

public interface IDbItem<TId>
{
    TId Id { get; init; }
    DateTime CreationDate { get; init; }
}
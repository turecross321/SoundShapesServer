namespace SoundShapesServer.Types.Database;

public interface IDbItem<TId>
{
    TId Id { get; init; }
    DateTimeOffset CreationDate { get; init; }
}
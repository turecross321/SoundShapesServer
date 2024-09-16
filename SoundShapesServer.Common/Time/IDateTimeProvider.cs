namespace SoundShapesServer.Common.Time;

public interface IDateTimeProvider
{
    public DateTimeOffset Now { get; }
}
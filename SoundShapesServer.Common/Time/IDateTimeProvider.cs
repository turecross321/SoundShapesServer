namespace SoundShapesServer.Common.Time;

public interface IDateTimeProvider
{
    public DateTime Now { get; }
}
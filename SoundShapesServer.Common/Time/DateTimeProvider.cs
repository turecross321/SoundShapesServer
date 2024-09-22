namespace SoundShapesServer.Common.Time;

public class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
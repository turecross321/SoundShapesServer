using SoundShapesServer.Common.Time;

namespace SoundShapesServer.Tests.Server;

public class MockDateTimeProvider : IDateTimeProvider
{
    public DateTime Now { get; set; } = DateTime.UnixEpoch;
}
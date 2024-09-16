using SoundShapesServer.Common.Time;

namespace SoundShapesServer.Tests.Server;

public class MockDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now { get; set; } = DateTimeOffset.UnixEpoch;
}
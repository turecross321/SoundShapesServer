using Microsoft.EntityFrameworkCore;
using SoundShapesServer.Common.Time;
using SoundShapesServer.Database;
using SoundShapesServer.Tests.Server;

namespace SoundShapesServer.Tests.Database;

// todo: this sucks and doesnt even work
public class TestDatabaseContext(IDateTimeProvider time) : GameDatabaseContext("", time)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseInMemoryDatabase("MemoryDb");
}
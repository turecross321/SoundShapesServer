using Bunkum.Core.Database;
using Microsoft.EntityFrameworkCore;
using SoundShapesServer.Common.Time;
using SoundShapesServer.Common.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext: DbContext, IDatabaseContext
{
    public GameDatabaseContext()
    {
        this._connectionString = "";
        this._time = new SystemDateTimeProvider();
    }
    public GameDatabaseContext(string connection, IDateTimeProvider timeProvider)
    {
        this._connectionString = connection;
        this._time = timeProvider;
    }

    private readonly IDateTimeProvider _time;
    public IDateTimeProvider Time => _time;

    private readonly string? _connectionString;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(this._connectionString);
    
    private DbSet<DbUser> Users { get; set; }
    private DbSet<DbCode> Codes { get; set; }
    private DbSet<DbToken> Tokens { get; set; }

    public override void Dispose()
    {
        SaveChanges();
        base.Dispose();
    }
}
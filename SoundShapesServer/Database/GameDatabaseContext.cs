using Bunkum.Core.Database;
using Microsoft.EntityFrameworkCore;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext(ServerConfig config) : DbContext, IDatabaseContext
{
    
    private readonly ServerConfig _config = config;

    public GameDatabaseContext() : this(new ServerConfig())
    {
    }
    
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
    
    private DbSet<DbUser> Users { get; set; }
    private DbSet<DbCodeToken> CodeTokens { get; set; }
    private DbSet<DbToken> Tokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(config.PostgresSqlConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public override void Dispose()
    {
        SaveChanges();
        base.Dispose();
    }
}
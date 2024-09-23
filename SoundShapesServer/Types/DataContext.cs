using NotEnoughLogs;
using SoundShapesServer.Database;

namespace SoundShapesServer.Types;

public class DataContext
{
    public required GameDatabaseContext Database;
    public required Logger Logger;
}
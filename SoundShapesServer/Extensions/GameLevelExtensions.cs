using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Extensions;

public static class GameLevelExtensions
{
    public static bool HasUserAccess(this GameLevel level, GameUser? accessor)
    {
        return !(level.Visibility == LevelVisibility.Private && level.Author.Id != accessor?.Id && accessor?.PermissionsType < PermissionsType.Moderator);
    }
}
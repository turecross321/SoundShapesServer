using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class PermissionHelper
{
    public static bool IsUserAdmin(GameUser user)
    {
        return user.PermissionsType == PermissionsType.Administrator;
    }

    public static bool IsUserModeratorOrMore(GameUser user)
    {
        return user.PermissionsType >= PermissionsType.Moderator;
    }
}
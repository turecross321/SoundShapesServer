using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class PermissionHelper
{
    public static bool IsUserAdmin(GameUser user)
    {
        return user.PermissionsType == (int)PermissionsType.Administrator;
    }

    public static bool IsUserModeratorOrMore(GameUser user)
    {
        return user.PermissionsType >= (int)PermissionsType.Moderator;
    }
}
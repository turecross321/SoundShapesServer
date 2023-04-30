using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class PermissionHelper
{
    public static bool IsUserAdmin(GameUser user)
    {
        return user.PermissionsType == (int)PermissionsType.Administrator;
    }
}
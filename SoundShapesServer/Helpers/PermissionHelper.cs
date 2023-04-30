using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class PermissionHelper
{
    public static bool IsUserAdmin(GameUser user)
    {
        return GameUser.Type == (int)UserType.Administrator;
    }
}
using Realms.Sync;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class PermissionHelper
{
    public static bool IsUserAdmin(GameUser user)
    {
        return user.Type == (int)UserType.Administrator;
    }
}
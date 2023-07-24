using SoundShapesServer.Types;

namespace SoundShapesServer.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class MinimumPermissionsAttribute : Attribute
{
    public readonly PermissionsType MinimumPermissions;

    public MinimumPermissionsAttribute(PermissionsType minimumPermissions) => MinimumPermissions = minimumPermissions;
}
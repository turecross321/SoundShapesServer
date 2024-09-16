using SoundShapesServer.Common.Types;

namespace SoundShapesServer.Attributes;

public class MinimumRoleAttribute(UserRole role) : Attribute
{
    public readonly UserRole MinimumRole = role;
}
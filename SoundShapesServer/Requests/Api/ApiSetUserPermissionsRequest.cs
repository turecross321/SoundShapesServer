using SoundShapesServer.Types;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetUserPermissionsRequest
{
    public PermissionsType PermissionsType { get; }
}
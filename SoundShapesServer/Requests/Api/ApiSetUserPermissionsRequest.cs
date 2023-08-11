// ReSharper disable UnassignedGetOnlyAutoProperty

using SoundShapesServer.Types;

namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetUserPermissionsRequest
{
    public PermissionsType PermissionsType { get; set; }
}
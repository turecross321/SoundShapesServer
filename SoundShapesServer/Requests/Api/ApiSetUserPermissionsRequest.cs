namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiSetUserPermissionsRequest
{
    public ApiSetUserPermissionsRequest(int permissionsType)
    {
        PermissionsType = permissionsType;
    }

    public int PermissionsType { get; }
}
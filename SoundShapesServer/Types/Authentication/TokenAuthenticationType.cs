namespace SoundShapesServer.Types.Authentication;

public enum TokenAuthenticationType
{
    Credentials = 0,
    PreExistingToken = 1,
    // ReSharper disable once IdentifierTypo
    Rpcn = 2,
    Psn = 3,
    Ip = 4,
    None = 5
}
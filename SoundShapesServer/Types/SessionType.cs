namespace SoundShapesServer.Types;
public enum SessionType
{
    Game = 0,
    API = 1,
    SetPassword = 2,
    SetEmail = 3,
    RemoveAccount = 4,
    Unauthorized = 5,
    Unknown = 6
}
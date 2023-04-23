namespace SoundShapesServer.Types;
public enum TypeOfSession
{
    Game = 0,
    API = 1,
    SetPassword = 2,
    SetEmail = 3,
    Unauthorized = 4,
    Unknown = 5
}